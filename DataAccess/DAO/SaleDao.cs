using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DataAccess.DAO
{
    public class SaleDao
    {
        public bool RegisterSale(Sale sale)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    foreach (var detail in sale.SaleDetails)
                    {
                        var product = context.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);

                        if (product == null || product.Stock < detail.Quantity)
                        {
                            throw new FaultException($"No hay suficiente stock para el producto con ID: {detail.ProductId}");
                        }

                        product.Stock -= detail.Quantity;
                        product.HasSales = true;

                        detail.UnitPrice = product.SalePrice;
                    }

                    context.Sales.Add(sale);
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al registrar la venta: " + ex.Message);
            }
        }

        public bool UpdateSale(Sale sale)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var existingSale = context.Sales
                        .Include(s => s.SaleDetails)
                        .FirstOrDefault(s => s.SaleId == sale.SaleId);

                    if (existingSale == null)
                        throw new FaultException("La venta no existe.");

                    // Revertir stock de los productos anteriores
                    foreach (var oldDetail in existingSale.SaleDetails)
                    {
                        var product = context.Products.FirstOrDefault(p => p.ProductId == oldDetail.ProductId);
                        if (product != null)
                        {
                            product.Stock += oldDetail.Quantity;
                        }
                    }

                    // Eliminar detalles anteriores
                    context.SaleDetails.RemoveRange(existingSale.SaleDetails);

                    // Agregar los nuevos detalles
                    existingSale.SaleDetails = new List<SaleDetail>();
                    foreach (var newDetail in sale.SaleDetails)
                    {
                        var product = context.Products.FirstOrDefault(p => p.ProductId == newDetail.ProductId);
                        if (product == null || product.Stock < newDetail.Quantity)
                        {
                            throw new FaultException($"No hay suficiente stock para el producto con ID: {newDetail.ProductId}");
                        }

                        product.Stock -= newDetail.Quantity;
                        newDetail.UnitPrice = product.SalePrice;

                        existingSale.SaleDetails.Add(newDetail);
                    }

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al actualizar la venta: " + ex.Message);
            }
        }

        public bool CancelSale(int saleId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var sale = context.Sales
                        .Include(s => s.SaleDetails)
                        .FirstOrDefault(s => s.SaleId == saleId);

                    if (sale == null)
                        throw new FaultException("La venta no existe.");

                    if (sale.IsCancelled)
                        return true; // ya estaba cancelada

                    sale.IsCancelled = true;

                    // Reponer stock
                    foreach (var detail in sale.SaleDetails)
                    {
                        var product = context.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                        if (product != null)
                        {
                            product.Stock += detail.Quantity;
                        }
                    }

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al cancelar la venta: " + ex.Message);
            }
        }

        public Sale GetSaleById(int saleId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Sales
                        .Include(s => s.SaleDetails.Select(d => d.Product))
                        .FirstOrDefault(s => s.SaleId == saleId);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al buscar la venta: " + ex.Message);
            }
        }
        public List<Sale> GetSales(string customerName = null, DateTime? date = null)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var query = context.Sales
                        .Include(s => s.SaleDetails.Select(d => d.Product))
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(customerName))
                    {
                        query = query.Where(s => s.CustomerName.Contains(customerName));
                    }

                    if (date.HasValue)
                    {
                        query = query.Where(s => DbFunctions.TruncateTime(s.SaleDate) == DbFunctions.TruncateTime(date.Value));
                    }

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al recuperar las ventas: " + ex.Message);
            }
        }

        public List<SalesByCategoryReport> GetSalesByCategoryReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var sales = context.Sales
                        .Include(s => s.SaleDetails.Select(d => d.Product))
                        .Where(s => DbFunctions.TruncateTime(s.SaleDate) >= DbFunctions.TruncateTime(startDate)
                                 && DbFunctions.TruncateTime(s.SaleDate) <= DbFunctions.TruncateTime(endDate)
                                 && !s.IsCancelled)
                        .ToList();

                    decimal totalRevenueGlobal = sales
                        .SelectMany(s => s.SaleDetails)
                        .Sum(d => d.Quantity * d.UnitPrice);

                    var grouped = sales
                        .SelectMany(s => s.SaleDetails)
                        .GroupBy(d => d.Product.Category)
                        .Select(g => new SalesByCategoryReport
                        {
                            Category = g.Key,
                            QuantitySold = g.Sum(x => x.Quantity),
                            SalesCount = g.Select(x => x.SaleId).Distinct().Count(),
                            TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice),
                            Percentage = totalRevenueGlobal > 0 ?
                                (double)(g.Sum(x => x.Quantity * x.UnitPrice) / totalRevenueGlobal) * 100 : 0,
                            TopProduct = g.GroupBy(x => x.Product.ProductName)
                                          .OrderByDescending(x => x.Sum(y => y.Quantity))
                                          .FirstOrDefault().Key
                        }).ToList();

                    return grouped;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al generar el reporte de ventas: " + ex.Message);
            }
        }
    }
}
