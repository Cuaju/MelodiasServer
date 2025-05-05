using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class SalesByCategoryReport
    {
        public string Category { get; set; }
        public int QuantitySold { get; set; }
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public double Percentage { get; set; }
        public string TopProduct { get; set; }
    }
}