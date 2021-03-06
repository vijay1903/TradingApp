﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using TradingApp.Models;
using Newtonsoft.Json;

namespace TradingApp.Infrastructure.TradingAppHandler
{
    public class IEXHandler
    {
        static string BASE_URL = "https://api.iextrading.com/1.0/"; //This is the base URL, method specific URL is appended to this.
        HttpClient httpClient;

        public IEXHandler()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /****
         * Calls the IEX reference API to get the list of symbols. 
        ****/
        public List<Company> GetSymbols()
        {
            string TradingApp_API_PATH = BASE_URL + "ref-data/symbols";
            string companyList = "";

            List<Company> companies = null;

            httpClient.BaseAddress = new Uri(TradingApp_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(TradingApp_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                companyList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!companyList.Equals(""))
            {
                companies = JsonConvert.DeserializeObject<List<Company>>(companyList);
            }
            return companies;
        }

        /****
         * Calls the IEX stock API to get 1 year's chart for the supplied symbol. 
        ****/
        public List<Stock> GetChart(string symbol, string range)
        {
            // Using the format method.
            // string TradingApp_API_PATH = BASE_URL + "stock/{0}/batch?types=chart&range=1y";
            // TradingApp_API_PATH = string.Format(TradingApp_API_PATH, symbol);

            string TradingApp_API_PATH = BASE_URL + "stock/" + Uri.EscapeDataString(symbol) + "/batch?types=chart&range="+ Uri.EscapeDataString(range);

            string charts = "";
            List<Stock> Stocks = new List<Stock>();
            httpClient.BaseAddress = new Uri(TradingApp_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(TradingApp_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                charts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            if (!charts.Equals(""))
            {
                ChartRoot root = JsonConvert.DeserializeObject<ChartRoot>(charts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Stocks = root.chart.ToList();
            }
            //make sure to add the symbol the chart
            foreach (Stock Stock in Stocks)
            {
                Stock.symbol = symbol;
            }

            return Stocks;
        }
        
        public Information GetInfo(string symbol)
        {
            string api = BASE_URL + "/stock/" + symbol + "/company";
            string info = "";
            httpClient.BaseAddress = new Uri(api);
            HttpResponseMessage response = httpClient.GetAsync(api).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                info = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            if (!info.Equals(""))
            {
                return(JsonConvert.DeserializeObject<Information>(info));
            }
            return null;
        }

        public List<Stock> GetTimeSeries(string symbol)
        {
            string TradingApp_API_PATH = BASE_URL + "stock/" + symbol + "/batch?types=chart&range=1m";

            string charts = "";
            List<Stock> Stocks = new List<Stock>();
            httpClient.BaseAddress = new Uri(TradingApp_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(TradingApp_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                charts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            if (!charts.Equals(""))
            {
                ChartRoot root = JsonConvert.DeserializeObject<ChartRoot>(charts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Stocks = root.chart.TakeLast(5).ToList();
            }
            //make sure to add the symbol the chart
            foreach (Stock Stock in Stocks)
            {
                Stock.symbol = symbol;
            }
            return Stocks;
        }
    }
}
