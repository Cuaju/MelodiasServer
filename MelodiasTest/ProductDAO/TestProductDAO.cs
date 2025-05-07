﻿using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using Xunit;

namespace MelodiasTests.ProductDAO
{
    public class TestProductDao
    {
        private Product CreateValidProduct(string suffix, bool hasSales = false)
        {
            return new Product
            {
                ProductName = $"Prod_{suffix}",
                ProductCode = $"PC_{suffix}",
                Description = "Description",
                PurchasePrice = 10,
                SalePrice = 15,
                Category = "Category",
                Brand = "Brand",
                Model = "Model",
                Stock = 10,
                Photo = "photo.jpg",
                Status = true,
                HasSales = hasSales
            };
        }

        [Fact]
        public void DeleteProduct_ReturnsTrue_WhenNoSales()
        {
            using (var scope = new TransactionScope())
            {
                int id;
                using (var ctx = new MelodiasContext())
                {
                    var prod = CreateValidProduct(Guid.NewGuid().ToString("N"));
                    ctx.Products.Add(prod);
                    ctx.SaveChanges();
                    id = prod.ProductId;
                }

                var dao = new ProductDao();
                var result = dao.DeleteProduct(id);
                Assert.True(result);
            }
        }

        [Fact]
        public void DeleteProduct_ThrowsFaultException_WhenHasSales()
        {
            using (var scope = new TransactionScope())
            {
                int id;
                using (var ctx = new MelodiasContext())
                {
                    var prod = CreateValidProduct(Guid.NewGuid().ToString("N"), true);
                    ctx.Products.Add(prod);
                    ctx.SaveChanges();
                    id = prod.ProductId;
                }

                var dao = new ProductDao();
                Assert.Throws<FaultException>(() => dao.DeleteProduct(id));
            }
        }

        [Fact]
        public void DeleteProduct_ThrowsFaultException_WhenNotFound()
        {
            var dao = new ProductDao();
            Assert.Throws<FaultException>(() => dao.DeleteProduct(-999));
        }

        [Fact]
        public void ExistsProductByName_ReturnsTrue_WhenDuplicateExists()
        {
            using (var scope = new TransactionScope())
            {
                var suffix = Guid.NewGuid().ToString("N");
                using (var ctx = new MelodiasContext())
                {
                    var prod = CreateValidProduct(suffix);
                    ctx.Products.Add(prod);
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                Assert.True(dao.ExistsProductByName($"Prod_{suffix}", -1));
            }
        }

        [Fact]
        public void ExistsProductByName_ReturnsFalse_WhenNameIsUnique()
        {
            var dao = new ProductDao();
            Assert.False(dao.ExistsProductByName("UniqueName_" + Guid.NewGuid(), -1));
        }

        [Fact]
        public void GetProductsList_ReturnsEmpty_WhenNoMatch()
        {
            using (var scope = new TransactionScope())
            {
                var dao = new ProductDao();
                var result = dao.GetProductsList("no_match_12345");
                Assert.Empty(result);
            }
        }

        [Fact]
        public void GetProductsList_ReturnsMatchingProducts()
        {
            using (var scope = new TransactionScope())
            {
                var token = Guid.NewGuid().ToString("N").Substring(0, 5);
                using (var ctx = new MelodiasContext())
                {
                    ctx.Products.Add(CreateValidProduct("A" + token));
                    ctx.Products.Add(CreateValidProduct("B" + token));
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var result = dao.GetProductsList(token);
                Assert.True(result.Count >= 2);
                Assert.All(result, p => Assert.Contains(token, p.ProductName));
            }
        }

        [Fact]
        public void RegisterProduct_ReturnsTrue_WhenValid()
        {
            using (var scope = new TransactionScope())
            {
                var dao = new ProductDao();
                var product = CreateValidProduct(Guid.NewGuid().ToString("N"));
                var result = dao.RegisterProduct(product);
                Assert.True(result);
            }
        }

        [Fact]
        public void RegisterProduct_ReturnsFalse_WhenDuplicateCode()
        {
            using (var scope = new TransactionScope())
            {
                var code = "P_DUPLICATE";

                using (var ctx = new MelodiasContext())
                {
                    var existingProduct = CreateValidProduct("DuplicateCodeTest");
                    existingProduct.ProductCode = code;
                    ctx.Products.Add(existingProduct);
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var product = CreateValidProduct(Guid.NewGuid().ToString("N"));
                product.ProductCode = code;

                var result = dao.RegisterProduct(product);
                Assert.False(result);
            }
        }


        [Fact]
        public void RegisterProduct_ThrowsFaultException_WhenDuplicateName()
        {
            using (var scope = new TransactionScope())
            {
                var name = "NameDup_" + Guid.NewGuid();
                using (var ctx = new MelodiasContext())
                {
                    ctx.Products.Add(CreateValidProduct(name));
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var dupProduct = CreateValidProduct(name);
                Assert.Throws<FaultException>(() => dao.RegisterProduct(dupProduct));
            }
        }

        [Fact]
        public void GetProducts_ReturnsOnlyActive()
        {
            using (var scope = new TransactionScope())
            {
                using (var ctx = new MelodiasContext())
                {
                    ctx.Products.Add(CreateValidProduct("Active1"));
                    var inactive = CreateValidProduct("Inactive1");
                    inactive.Status = false;
                    ctx.Products.Add(inactive);
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var products = dao.GetProducts();
                Assert.All(products, p => Assert.True(p.Status));
            }
        }

        [Fact]
        public void SearchProducts_ReturnsResults_WhenFiltered()
        {
            using (var scope = new TransactionScope())
            {
                var token = Guid.NewGuid().ToString("N").Substring(0, 5);
                using (var ctx = new MelodiasContext())
                {
                    ctx.Products.Add(new Product
                    {
                        ProductName = $"Prod_{token}",
                        ProductCode = $"Code_{token}",
                        Category = "Cat",
                        Brand = "Brand",
                        Description = "Desc",
                        PurchasePrice = 10,
                        SalePrice = 20,
                        Model = "Model",
                        Stock = 5,
                        Photo = "img.jpg",
                        Status = true,
                        HasSales = false
                    });
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var result = dao.SearchProducts($"Prod_{token}", $"Code_{token}", "Cat", "Brand");
                Assert.Single(result);
            }
        }

        [Fact]
        public void SearchProducts_ReturnsAll_WhenNoFilters()
        {
            using (var scope = new TransactionScope())
            {
                using (var ctx = new MelodiasContext())
                {
                    ctx.Products.Add(CreateValidProduct("F1"));
                    ctx.Products.Add(CreateValidProduct("F2"));
                    ctx.SaveChanges();
                }

                var dao = new ProductDao();
                var result = dao.SearchProducts(null, null, null, null);
                Assert.True(result.Count >= 2);
            }
        }
    }
}
