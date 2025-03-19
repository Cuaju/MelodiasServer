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
        bool EditProduct(ProductDataContract product);

        [OperationContract]
        bool ExistsProductByName(string productName, int productId);

        [OperationContract]
        bool DeleteProduct(int productId);

    }

    [DataContract]
    public class ProductDataContract
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ProductCode{ get; set; }

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
    }
}