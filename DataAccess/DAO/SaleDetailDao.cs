using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DataAccess.DAO
{
    public class SaleDetailDao
    {
        public List<SaleDetail> GetDetailsBySaleId(int saleId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.SaleDetails
                        .Include(d => d.Product)
                        .Where(d => d.SaleId == saleId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al obtener detalles de la venta: " + ex.Message);
            }
        }

        public SaleDetail GetDetailById(int detailId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.SaleDetails
                        .Include(d => d.Product)
                        .FirstOrDefault(d => d.SaleDetailId == detailId);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al obtener el detalle de venta: " + ex.Message);
            }
        }

        public bool DeleteDetail(int detailId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var detail = context.SaleDetails.FirstOrDefault(d => d.SaleDetailId == detailId);

                    if (detail == null)
                        throw new FaultException("El detalle de venta no existe.");

                    // Restaurar stock
                    var product = context.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                    if (product != null)
                    {
                        product.Stock += detail.Quantity;
                    }

                    context.SaleDetails.Remove(detail);
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al eliminar el detalle de venta: " + ex.Message);
            }
        }

        public bool UpdateDetail(SaleDetail updatedDetail)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var existingDetail = context.SaleDetails.FirstOrDefault(d => d.SaleDetailId == updatedDetail.SaleDetailId);

                    if (existingDetail == null)
                        throw new FaultException("El detalle de venta no existe.");

                    // Restaurar stock antiguo
                    var oldProduct = context.Products.FirstOrDefault(p => p.ProductId == existingDetail.ProductId);
                    if (oldProduct != null)
                    {
                        oldProduct.Stock += existingDetail.Quantity;
                    }

                    // Verificar y ajustar nuevo stock
                    var newProduct = context.Products.FirstOrDefault(p => p.ProductId == updatedDetail.ProductId);
                    if (newProduct == null || newProduct.Stock < updatedDetail.Quantity)
                        throw new FaultException("No hay suficiente stock para actualizar el producto.");

                    newProduct.Stock -= updatedDetail.Quantity;

                    // Actualizar valores
                    existingDetail.ProductId = updatedDetail.ProductId;
                    existingDetail.Quantity = updatedDetail.Quantity;
                    existingDetail.UnitPrice = updatedDetail.UnitPrice;

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al actualizar el detalle de venta: " + ex.Message);
            }
        }
    }
}
