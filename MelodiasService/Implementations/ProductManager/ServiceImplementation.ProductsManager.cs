using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService.Implementations
{
    public partial class ServiceImplementation : IProductsManager
    {
        private ProductDao productDao = new ProductDao();

        public bool RegisterProduct(ProductDataContract product)
        {
            try
            {
                if (productDao.ExistsProductByName(product.ProductName, product.ProductId))
                {
                    throw new FaultException("The product name is already in use by another product.");
                }

                Product newProduct = new Product
                {
                    ProductName = product.ProductName,
                    ProductCode = product.ProductCode,
                    Description = product.Description,
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    Category = product.Category,
                    Brand = product.Brand,
                    Model = product.Model,
                    Stock = product.Stock,
                    Photo = product.Photo,
                    Status = true
                };

                bool registered = productDao.RegisterProduct(newProduct);

                if (!registered)
                    throw new FaultException("The product already exists in the database.");

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

        public List<ProductDataContract> GetProducts()
        {
            try
            {
                List<Product> products = productDao.GetProducts();

                return products.Select(p => new ProductDataContract
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductCode = p.ProductCode,
                    Description = p.Description,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    Category = p.Category,
                    Brand = p.Brand,
                    Model = p.Model,
                    Stock = p.Stock,
                    Photo = p.Photo,
                    Status = p.Status,
                    HasSales = p.HasSales
                }).ToList();
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

        public bool EditProduct(ProductDataContract product)
        {
            try
            {
                Product existingProduct = new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductCode = product.ProductCode,
                    Description = product.Description,
                    PurchasePrice = product.PurchasePrice,
                    SalePrice = product.SalePrice,
                    Category = product.Category,
                    Brand = product.Brand,
                    Model = product.Model,
                    Stock = product.Stock,
                    Photo = product.Photo,
                    Status = product.Status
                };

                bool updated = productDao.UpdateProduct(existingProduct);

                if (!updated)
                {
                    throw new FaultException("The product could not be updated.");
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

        public bool ExistsProductByName(string productName, int productId)
        {
            try
            {
                return productDao.ExistsProductByName(productName, productId);
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

        public bool DeleteProduct(int productId)
        {
            try
            {
                bool deleted = productDao.DeleteProduct(productId);
                if (!deleted)
                {
                    throw new FaultException("The product could not be deleted.");
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