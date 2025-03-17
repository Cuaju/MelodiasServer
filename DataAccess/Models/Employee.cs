using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DataAccess.Models
{
    public class Employee
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surnames { get; set; }

        public int Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }

        public string Password { get; set; }

    }
}
