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
    public class ProductDao
    {
        public bool RegisterProduct(Product product)
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

        public List<Product> GetProducts()
        {
            using (var context = new MelodiasContext())
            {
                return context.Products.Where(p => p.Status).ToList();
            }
        }

        public bool UpdateProduct(Product product)
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
            using (var context = new MelodiasContext())
            {
                return context.Products.Any(p => p.ProductName == productName && p.ProductId != productId);
            }
        }

        public bool DeleteProduct(int productId)
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
    }
}