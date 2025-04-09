using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace MelodiasService.Implementations
{
    public partial class ServiceImplementation : ISalesManager
    {
        private SaleDao saleDao = new SaleDao();

        public bool RegisterSale(SaleDataContract saleDC)
        {
            try
            {
                Sale sale = new Sale
                {
                    CustomerName = saleDC.CustomerName,
                    SaleDate = saleDC.SaleDate,
                    SaleDetails = saleDC.SaleDetails.Select(d => new SaleDetail
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity
                        // UnitPrice será asignado por el DAO
                    }).ToList()
                };

                return saleDao.RegisterSale(sale);
            }
            catch (FaultException ex)
            {
                throw new FaultException("Error de validación: " + ex.Message);
            }
            catch (CommunicationException)
            {
                throw new FaultException("Error de comunicación con el servidor. Intente más tarde.");
            }
            catch (TimeoutException)
            {
                throw new FaultException("Tiempo de espera agotado. El servidor no respondió.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado en el servidor: " + ex.Message);
            }
        }

        public bool UpdateSale(SaleDataContract saleDC)
        {
            try
            {
                Sale sale = new Sale
                {
                    SaleId = saleDC.SaleId,
                    CustomerName = saleDC.CustomerName,
                    SaleDate = saleDC.SaleDate,
                    SaleDetails = saleDC.SaleDetails.Select(d => new SaleDetail
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity
                    }).ToList()
                };

                return saleDao.UpdateSale(sale);
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
                return saleDao.CancelSale(saleId);
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al cancelar la venta: " + ex.Message);
            }
        }

        public SaleDataContract GetSaleById(int saleId)
        {
            try
            {
                Sale sale = saleDao.GetSaleById(saleId);

                if (sale == null)
                    throw new FaultException("La venta no existe.");

                return new SaleDataContract
                {
                    SaleId = sale.SaleId,
                    SaleDate = sale.SaleDate,
                    CustomerName = sale.CustomerName,
                    IsCancelled = sale.IsCancelled,
                    SaleDetails = sale.SaleDetails.Select(d => new SaleDetailDataContract
                    {
                        ProductId = d.ProductId,
                        ProductName = d.Product?.ProductName ?? "",
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new FaultException("Error al recuperar la venta: " + ex.Message);
            }
        }

        public bool EditSale(SaleDataContract sale)
        {
            try
            {
                var updatedSale = new Sale
                {
                    SaleId = sale.SaleId,
                    CustomerName = sale.CustomerName,
                    SaleDate = sale.SaleDate,
                    IsCancelled = sale.IsCancelled,
                    SaleDetails = sale.SaleDetails.Select(detail => new SaleDetail
                    {
                        SaleDetailId = detail.SaleDetailId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice
                    }).ToList()
                };

                bool result = saleDao.UpdateSale(updatedSale);

                if (!result)
                {
                    throw new FaultException("No se pudo actualizar la venta.");
                }

                return true;
            }
            catch (FaultException ex)
            {
                throw new FaultException("Error de validación: " + ex.Message);
            }
            catch (CommunicationException)
            {
                throw new FaultException("Error de comunicación con el servidor. Intente más tarde.");
            }
            catch (TimeoutException)
            {
                throw new FaultException("Tiempo de espera agotado. El servidor no respondió.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado en el servidor: " + ex.Message);
            }
        }

    }
}
