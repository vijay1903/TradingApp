using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class CompanyChange
    {
        [Key]
        public string Symbol { get; set; }
        public string Name { get; set; }
        public float AveragePercentChange { get; set; }
        public CompanyChange(String symbol, String name, float change)
        {
            Symbol = symbol;
            Name = name;
            AveragePercentChange = change;
        }
        public CompanyChange()
        {

        }
    }
}
