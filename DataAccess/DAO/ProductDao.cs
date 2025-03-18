using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDao
    {
        public bool RegisterProduct(Product product)
        {
            using (var context = new MelodiasContext())
            {
                if (context.Products.Any(p => p.ProductCode == product.ProductCode))
                    return false;

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
                    return false;

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
    }
}