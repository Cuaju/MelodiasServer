using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService
{
    [ServiceContract]
    public interface IProductsManager
    {
        [OperationContract]
        bool RegisterProduct(ProductDataContract product);

        [OperationContract]
        List<ProductDataContract> GetProducts();
        
        [OperationContract]
        List<ProductDataContract> GetProductsSearch(string search);

        [OperationContract]
        bool EditProduct(ProductDataContract product);

        [OperationContract]
        bool ExistsProductByName(string productName, int productId);

        [OperationContract]
        bool DeleteProduct(int productId);

        [OperationContract]
        List<ProductDataContract> SearchProducts(string name, string code, string category, string brand);

        [OperationContract]
        List<SalesByCategoryReport> GetSalesByCategoryReport(DateTime startDate, DateTime endDate);

        [OperationContract]
        List<SalesByProductReport> GetSalesByProductReport(DateTime startDate, DateTime endDate);
    }

    [DataContract]
    public class ProductDataContract
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ProductCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal PurchasePrice { get; set; }

        [DataMember]
        public decimal SalePrice { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public int Stock { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool HasSales { get; set; }


        public static explicit operator ProductDataContract(ProductData productData)
        {
            if (productData == null)
            {
                throw new ArgumentNullException(nameof(productData));
            }

            return new ProductDataContract
            {
                ProductId = productData.ProductId,
                ProductName = productData.ProductName,
                ProductCode = productData.ProductCode,
                Description = productData.Description,
                PurchasePrice = productData.PurchasePrice,
                SalePrice = productData.SalePrice,
                Category = productData.Category,
                Brand = productData.Brand,
                Model = productData.Model,
                Stock = productData.Stock,
                Photo = productData.Photo,
                Status = productData.Status,
                HasSales = productData.HasSales
            };
        }

    }
}