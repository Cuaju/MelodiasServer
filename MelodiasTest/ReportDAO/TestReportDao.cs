using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Xunit;

namespace MelodiasTests.ReportDAO
{
    public class TestRerportDao
    {
        private Product CreateProduct(string name, string category)
        {
            return new Product
            {
                ProductName = name,
                ProductCode = "CODE_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Description = "Test",
                PurchasePrice = 10,
                SalePrice = 15,
                Category = category,
                Brand = "Brand",
                Model = "Model",
                Stock = 100,
                Photo = "img.png",
                Status = true,
                HasSales = false
            };
        }

        [Fact]
        public void GetSalesByCategoryReport_ReturnsCorrectData()
        {
            using (var scope = new TransactionScope())
            {
                var context = new MelodiasContext();

                var prod1 = CreateProduct("ProdCat1_A", "Cat1");
                var prod2 = CreateProduct("ProdCat1_B", "Cat1");
                var prod3 = CreateProduct("ProdCat2", "Cat2");

                context.Products.AddRange(new[] { prod1, prod2, prod3 });
                context.SaveChanges();

                var sale = new Sale
                {
                    CustomerName = "Cliente 1",
                    SaleDate = DateTime.Today,
                    IsCancelled = false,
                    SaleDetails = new List<SaleDetail>
                    {
                        new SaleDetail { ProductId = prod1.ProductId, Quantity = 3, UnitPrice = prod1.SalePrice },
                        new SaleDetail { ProductId = prod2.ProductId, Quantity = 2, UnitPrice = prod2.SalePrice },
                        new SaleDetail { ProductId = prod3.ProductId, Quantity = 5, UnitPrice = prod3.SalePrice }
                    }
                };

                context.Sales.Add(sale);
                context.SaveChanges();

                var dao = new SaleDao();
                var report = dao.GetSalesByCategoryReport(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

                Assert.Equal(2, report.Count);
                Assert.Contains(report, r => r.Category == "Cat1");
                Assert.Contains(report, r => r.Category == "Cat2");

                var cat1Report = report.First(r => r.Category == "Cat1");
                Assert.Equal(5, cat1Report.QuantitySold);
                Assert.Equal(1, cat1Report.SalesCount);
                Assert.Equal(5 * 15, cat1Report.TotalRevenue);

                var cat2Report = report.First(r => r.Category == "Cat2");
                Assert.Equal(5, cat2Report.QuantitySold);
                Assert.Equal(1, cat2Report.SalesCount);
                Assert.Equal(5 * 15, cat2Report.TotalRevenue);

                double totalPercentage = report.Sum(r => r.Percentage);
                Assert.InRange(totalPercentage, 99.9, 100.1);
            }
        }

        [Fact]
        public void GetSalesByProductReport_ReturnsCorrectData()
        {
            using (var scope = new TransactionScope())
            {
                var context = new MelodiasContext();
                var prod = CreateProduct("ProductoTest", "CatX");
                context.Products.Add(prod);
                context.SaveChanges();

                var sale = new Sale
                {
                    CustomerName = "Cliente",
                    SaleDate = DateTime.Today,
                    IsCancelled = false,
                    SaleDetails = new List<SaleDetail>
                    {
                        new SaleDetail { ProductId = prod.ProductId, Quantity = 4, UnitPrice = prod.SalePrice }
                    }
                };

                context.Sales.Add(sale);
                context.SaveChanges();

                var dao = new SaleDao();
                var report = dao.GetSalesByProductReport(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

                Assert.NotEmpty(report);
                var prodReport = report.FirstOrDefault(r => r.ProductName == "ProductoTest");
                Assert.NotNull(prodReport);
                Assert.Equal(4, prodReport.QuantitySold);
                Assert.Equal(1, prodReport.SalesCount);
                Assert.Equal(4 * prod.SalePrice, prodReport.TotalRevenue);
            }
        }

        [Fact]
        public void GetEarningsReport_ReturnsCorrectCalculations()
        {
            using (var scope = new TransactionScope())
            {
                var context = new MelodiasContext();
                var prod = CreateProduct("GananciaTest", "CatY");
                context.Products.Add(prod);
                context.SaveChanges();

                var sale = new Sale
                {
                    CustomerName = "Cliente",
                    SaleDate = DateTime.Today,
                    IsCancelled = false,
                    SaleDetails = new List<SaleDetail>
                    {
                        new SaleDetail { ProductId = prod.ProductId, Quantity = 2, UnitPrice = prod.SalePrice }
                    }
                };

                context.Sales.Add(sale);
                context.SaveChanges();

                var dao = new SaleDao();
                var report = dao.GetEarningsReport(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

                Assert.Equal(2 * prod.SalePrice, report.GrossEarnings);
                Assert.Equal(2 * (prod.SalePrice - prod.PurchasePrice), report.NetEarnings);
                Assert.Equal(2, report.TotalItemsSold);
                Assert.Equal(1, report.TotalSales);
            }
        }
    }
}
