using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Stock { get; set; }
        public string Photo { get; set; }
        public bool Status { get; set; }
        public bool HasSales { get; set; }
    }

    public class ProductDao
    {
        public List<ProductData> GetProductsList(string searchTerm)
        {
            using (var context = new MelodiasContext())
            {
                var normalizedSearchTerm = searchTerm.Trim().ToLower();

                var products = context.Products
                    .Where(p => p.ProductName.ToLower().Contains(normalizedSearchTerm) ||
                                p.ProductCode.ToLower().Contains(normalizedSearchTerm) ||
                                (p.Description != null && p.Description.ToLower().Contains(normalizedSearchTerm)))
                    .Take(10)
                    .ToList();

                var productDataList = products.Select(p => new ProductData
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

                return productDataList;
            }
        }


        public bool RegisterProduct(Product product)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    bool nameExists = context.Products.Any(p => p.ProductName == product.ProductName && p.ProductId != product.ProductId);
                    if (nameExists)
                    {
                        throw new FaultException("A product with this name already exists.");
                    }

                    if (context.Products.Any(p => p.ProductCode == product.ProductCode))
                    {
                        return false;
                    }

                    string nextCode = GenerateNextProductCode(context);
                    product.ProductCode = nextCode;

                    context.Products.Add(product);
                    return context.SaveChanges() > 0;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al registrar el producto: " + ex.Message);
            }
        }

        public List<Product> GetProducts()
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Products.Where(p => p.Status).ToList();
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al obtener los productos el producto: " + ex.Message);
            }
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var existingProduct = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (existingProduct == null)
                    {
                        return false;
                    }

                    if (context.Products.Any(p => p.ProductId != product.ProductId && p.ProductName.ToLower() == product.ProductName.ToLower()))
                    {
                        throw new FaultException("A product with this name already exists.");
                    }

                    if (existingProduct.ProductName == product.ProductName &&
                    existingProduct.Description == product.Description &&
                    existingProduct.PurchasePrice == product.PurchasePrice &&
                    existingProduct.SalePrice == product.SalePrice &&
                    existingProduct.Category == product.Category &&
                    existingProduct.Brand == product.Brand &&
                    existingProduct.Model == product.Model &&
                    existingProduct.Stock == product.Stock &&
                    existingProduct.Photo == product.Photo &&
                    existingProduct.Status == product.Status)
                    {
                        return true;
                    }

                    existingProduct.ProductName = product.ProductName;
                    existingProduct.Description = product.Description;
                    existingProduct.PurchasePrice = product.PurchasePrice;
                    existingProduct.SalePrice = product.SalePrice;
                    existingProduct.Category = product.Category;
                    existingProduct.Brand = product.Brand;
                    existingProduct.Model = product.Model;
                    existingProduct.Stock = product.Stock;
                    existingProduct.Photo = product.Photo;

                    return context.SaveChanges() > 0;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al actualizar el producto: " + ex.Message);
            }
        }

        private string GenerateNextProductCode(MelodiasContext context)
        {
            var lastProduct = context.Products.OrderByDescending(p => p.ProductId).FirstOrDefault();

            if (lastProduct != null && Regex.IsMatch(lastProduct.ProductCode, @"^P\d{4}$"))
            {
                int lastNumber = int.Parse(lastProduct.ProductCode.Substring(1));
                return $"P{(lastNumber + 1).ToString("D4")}";
            }

            return "P0001";
        }

        public bool ExistsProductByName(string productName, int productId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Products.Any(p => p.ProductName == productName && p.ProductId != productId);
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al verificar el nombre del producto: " + ex.Message);
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (product == null)
                    {
                        throw new FaultException("El producto no existe.");
                    }

                    if (product.HasSales)
                    {
                        throw new FaultException("Este producto no puede ser eliminado porque ya ha sido registrado en ventas.");
                    }

                    product.Status = false;
                    return context.SaveChanges() > 0;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al eliminar el producto: " + ex.Message);
            }
        }

        public List<Product> SearchProducts(string name, string code, string category, string brand)
        {
            using (var context = new MelodiasContext())
            {
                var query = context.Products.Where(p => p.Status);

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(p => p.ProductName.Contains(name));

                if (!string.IsNullOrWhiteSpace(code))
                    query = query.Where(p => p.ProductCode.Contains(code));

                if (!string.IsNullOrWhiteSpace(category) && category != "Seleccionar")
                    query = query.Where(p => p.Category == category);

                if (!string.IsNullOrWhiteSpace(brand) && brand != "Seleccionar")
                    query = query.Where(p => p.Brand == brand);

                return query.ToList();
            }
        }
    }
}