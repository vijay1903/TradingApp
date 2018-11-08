using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models.ViewModal
{
    public class CompaniesStocks
    {
        public List<Company> Companies { get; set; }
        public Stock Current { get; set; }
        public string Dates { get; set; }
        public string Prices { get; set; }
        public string Volumes { get; set; }
        public float AvgPrice { get; set; }
        public double AvgVolume { get; set; }

        public CompaniesStocks(List<Company> companies, Stock current, string dates, string prices, string volumes, float avgprice, double avgvolume)
        {
            Companies = companies;
            Current = current;
            Dates = dates;
            Prices = prices;
            Volumes = volumes;
            AvgPrice = avgprice;
            AvgVolume = avgvolume;
        }
    }
}
