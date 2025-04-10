using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService.DTOs
{
    [DataContract]
    public class PurchaseDTO
    {
        [DataMember]
        public int PurchaseId { get; set; }
        [DataMember]
        public int SupplierId { get; set; }
        [DataMember]    
        public int ProductId { get; set; }
        [DataMember]    
        public DateTime PurchaseDate { get; set; }
        [DataMember]    
        public decimal TotalCost { get; set; }
    }
}
