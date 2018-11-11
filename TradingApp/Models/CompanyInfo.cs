using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class CompanyInfo
    {
        public List<Company> Companies { get; set; }
        public Information Info { get; set; }

        public CompanyInfo(List<Company> companies, Information info)
        {
            Companies = companies;
            Info = info;
        }
    }
    public class Information
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Exchange { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string CEO { get; set; }
        public string Sector { get; set; }
        public string[] Tags { get; set; }
        public Information(string symbol, string name, string exchange, string industry, string website, string description, string ceo, string sector, string[] tags)
        {
            Symbol = symbol;
            name = CompanyName;
            Exchange = exchange;
            Industry = industry;
            Website = website;
            Description = description;
            CEO = ceo;
            Sector = sector;
            Tags = tags;
        }
    }
}
