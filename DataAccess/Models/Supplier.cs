using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Supplier
    {
            [Key]
            public int supplierId { get; set; }

            [Required]
            [Index("IX_SupplierCompanyName", IsUnique = true)]
            [MaxLength(100)]
            public string Name { get; set; }

            [Required]
            [MaxLength(200)]
            public string Address { get; set; }

            [Required]
            [MaxLength(10)]
            public string PostalCode { get; set; }

            [Required]
            [MaxLength(50)]
            public string City { get; set; }

            [Required]
            [MaxLength(50)]
            public string Country { get; set; }

            [Required]
            [MaxLength(20)]
            public string Phone { get; set; }

            [Required]
            [Index("IX_SupplierCompanyEmail", IsUnique = true)]
            [MaxLength(100)]
            public string Email { get; set; }
    }

}
