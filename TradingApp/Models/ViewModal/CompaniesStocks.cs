﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models.ViewModal
{
    public class CompaniesStocks
    {
        public List<Company> Companies { get; set; }
        public List<CompanyChange> Top5 { get; set; }
        public List<CompanyChange> Bottom5 { get; set; }
        public Stock Current { get; set; }
        public string Dates { get; set; }
        //public string Prices { get; set; }
        public string Opens { get; set; }
        public string Lows { get; set; }
        public string Highs { get; set; }
        public string Closes { get; set; }
        public string Volumes { get; set; }
        public string Changes { get; set; }
        public string PercentageChanges { get; set; }
        public string ChangesOverTime { get; set; }
        public float AvgPrice { get; set; }
        public double AvgVolume { get; set; }

        public CompaniesStocks(List<Company> companies, List<CompanyChange> top5, List<CompanyChange> bottom5, Stock current, string dates, string opens, string highs, string lows, string closes, string volumes, string changes, string percentChanges, string changesovertime, float avgprice, double avgvolume)
        {
            Companies = companies;
            Top5 = top5;
            Bottom5 = bottom5;
            Current = current;
            Dates = dates;
            Opens = opens;
            Lows = lows;
            Highs = highs;
            Closes = closes;
            Volumes = volumes;
            Changes = changes;
            PercentageChanges = percentChanges;
            ChangesOverTime = changesovertime;
            AvgPrice = avgprice;
            AvgVolume = avgvolume;
        }
    }
    public class CompareCompanies
    {
        public List<Company> Companies { get; set; }
        public List<CompareCompany> compareCompanies { get; set; }
        public CompareCompanies(List<Company> companies, List<CompareCompany> c)
        {
            Companies = companies;
            compareCompanies = c;
        }
    }
    public class CompareCompany
    {
        public string Symbol { get; set; }
        public Stock Current { get; set; }
        public string Dates { get; set; }
        public string Opens { get; set; }
        public string Lows { get; set; }
        public string Highs { get; set; }
        public string Closes { get; set; }
        public string Changes { get; set; }
        public string PercentageChanges { get; set; }
        public string ChangesOverTime { get; set; }
        public string Volumes { get; set; }
        public float AvgPrice { get; set; }
        public double AvgVolume { get; set; }

        public CompareCompany(Stock current,string symbol, string dates, string opens, string lows, string highs, string closes, string changes, string percentChanges, string changesovertime, string volumes, float avgprice, double avgvolume)
        {
            Symbol = symbol;
            Current = current;
            Dates = dates;
            Opens = opens;
            Lows = lows;
            Highs = highs;
            Closes = closes;
            Volumes = volumes;
            Changes = changes;
            PercentageChanges = percentChanges;
            ChangesOverTime = changesovertime;
            AvgPrice = avgprice;
            AvgVolume = avgvolume;
        }
    }
    
}
