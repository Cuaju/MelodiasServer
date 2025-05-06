using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class SalesByProductReport
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public int SalesCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
