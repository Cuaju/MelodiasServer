using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SalePrice { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; }

        [MaxLength(50)]
        public string Model { get; set; }

        [Required]
        public int Stock { get; set; }

        [MaxLength(255)]
        public string Photo { get; set; }

        [Required]
        public bool Status { get; set; } = true;
    }
}