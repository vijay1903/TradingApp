using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TradingApp.DataAccess;
using TradingApp.Infrastructure.TradingAppHandler;
using TradingApp.Models;
using TradingApp.Models.ViewModal;

namespace TradingApp.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IActionResult Index(String symbol,String range)
        {
            //Input: User logged In?, Chart Range, Chart Parameters, Company Symbol
            //Output: Chart Data, Stock List, Top stocks, ButtonName-{Sign In, Account}
            //ViewData["symbol"] = "aapl";
            
            //String symbol = ViewData["symbol"].ToString();
            if(range == null){
                range = "1m";
            }
            if(symbol == null)
            {
                symbol = "A";
            }

            ViewData["range"] = range;
            ViewData["symbol"] = symbol;
            ViewBag.dbSuccessChart = 0;
            //saveCompanies();
            List<Stock> Stocks = new List<Stock>();
            if (symbol != null)
            {
                IEXHandler webHandler = new IEXHandler();
                Stocks = webHandler.GetChart(symbol,range);
                Stocks = Stocks.OrderBy(c => c.date).ToList(); //Make sure the data is in ascending order of date.
            }

            CompaniesStocks companiesStocks = getCompaniesStocksModel(Stocks);
            return View(companiesStocks);
            //return View();
        }

        public IActionResult Compare()
        {
            //Input: Comparing Parameter, Stocks to compare, Chart Range
            //Output: Symbols, Chart Data

            return View();
        }

        public IActionResult Info()
        {
            // Input: Stock Symbol
            // Output: Stock company information

            return View();
        }

        public IActionResult Account()
        {
            // Input: isAuthenticated?
            // Output: SignIn/SignUp OR Accout Information, Interested stock list

            return View();
        }

        public IActionResult About()
        {
           //Static Page
           //Input: None
           //Output: None

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /****
        * Returns the ViewModel CompaniesStocks based on the data provided.
        ****/
        public IActionResult saveCompanies()
        {
            List<Company> companies = new List<Company>();
            IEXHandler webHandler = new IEXHandler();
            companies = webHandler.GetSymbols();
            companies = companies.OrderBy(c => c.symbol).ToList();
            foreach (Company company in companies)
            {
                if(dbContext.Companies.Where(c => c.symbol.Equals(company.symbol)).Count() == 0)
                {
                    dbContext.Companies.Add(company);
                }
            }
            dbContext.SaveChanges();
            ViewData["message"] = "Companies Refreshed!";
            return View("Index");
        }

        public CompaniesStocks getCompaniesStocksModel(List<Stock> Stocks)
        {
            List<Company> companies = dbContext.Companies.ToList();

            if (Stocks.Count == 0)
            {
                return new CompaniesStocks(companies, null, "", "", "", 0, 0);
            }

            Stock current = Stocks.Last();
            string dates = string.Join(",", Stocks.Select(e => e.date));
            string prices = string.Join(",", Stocks.Select(e => e.high));
            string volumes = string.Join(",", Stocks.Select(e => e.volume / 1000000)); //Divide vol by million
            float avgprice = Stocks.Average(e => e.high);
            double avgvol = Stocks.Average(e => e.volume) / 1000000; //Divide volume by million
            return new CompaniesStocks(companies, Stocks.Last(), dates, prices, volumes, avgprice, avgvol);
        }
        public IActionResult SaveCharts(string symbol)
        {
            IEXHandler webHandler = new IEXHandler();
            List<Stock> stocks = webHandler.GetChart(symbol, "1m");
            //List<Equity> equities = JsonConvert.DeserializeObject<List<Equity>>(TempData["Equities"].ToString());
            foreach (Stock stock in stocks)
            {
                if (dbContext.Stocks.Where(c => c.date.Equals(stock.date)).Count() == 0)
                {
                    dbContext.Stocks.Add(stock);
                }
            }

            dbContext.SaveChanges();
            ViewBag.dbSuccessChart = 1;

            CompaniesStocks companiesEquities = getCompaniesStocksModel(stocks);
            ViewData["message"] = "Chart saved successfully for "+symbol+" for 1 month.";
            return View("Index", companiesEquities);
        }
    }
}
