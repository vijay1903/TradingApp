using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class ChartRoot
    {
        public Stock[] chart { get; set; }
    }
    //public class ChartList
    //{
    //    public SymbolChart[] symbolChart {get; set;}
    //}
    //public class SymbolChart
    //{
    //    public String symbol { get; set; }
    //    public Stock[] chart { get; set; }
    //}
    public class Stock
    {
        public int StockId { get; set; }
        public string date { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }
        public float vwap { get; set; }
        public string label { get; set; }
        public float changeOverTime { get; set; }
        public string symbol { get; set; }
    }
}
