using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal TotalCost { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
