using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Employee
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string userName { get; set; }
    }
}
