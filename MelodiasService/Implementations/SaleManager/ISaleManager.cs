using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MelodiasService
{
    [ServiceContract]
    public interface ISalesManager
    {
        [OperationContract]
        bool RegisterSale(SaleDataContract sale);

        [OperationContract]
        bool EditSale(SaleDataContract sale);

        [OperationContract]
        bool CancelSale(int saleId);

        [OperationContract]
        SaleDataContract GetSaleById(int saleId);
    }

    [DataContract]
    public class SaleDataContract
    {
        [DataMember]
        public int SaleId { get; set; }

        [DataMember]
        public DateTime SaleDate { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public bool IsCancelled { get; set; }

        [DataMember]
        public List<SaleDetailDataContract> SaleDetails { get; set; }
    }

    [DataContract]
    public class SaleDetailDataContract
    {
        [DataMember]
        public int SaleDetailId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal Subtotal => UnitPrice * Quantity;
    }
}
