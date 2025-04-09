using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public bool IsCancelled { get; set; } = false;

        // Navegación: lista de productos vendidos
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
    }
}
