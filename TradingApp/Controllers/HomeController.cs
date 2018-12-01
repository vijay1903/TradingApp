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

        public IActionResult Index(String symbol, String range)
        {
            //Input: User logged In?, Chart Range, Chart Parameters, Company Symbol
            //Output: Chart Data, Stock List, Top stocks, ButtonName-{Sign In, Account}
            //ViewData["symbol"] = "aapl";

            //String symbol = ViewData["symbol"].ToString();
            if (range == null)
            {
                range = "1m";
            }
            if (symbol == null)
            {
                symbol = "A";
            }

            ViewData["range"] = range;
            ViewData["symbol"] = symbol;
            ViewBag.dbSuccessChart = 0;
            //SaveCompanies();
            List<Stock> Stocks = null;
            if (symbol != null)
            {
                IEXHandler webHandler = new IEXHandler();
                Stocks = webHandler.GetChart(symbol, range);
                /*Stocks = Stocks.OrderBy(c => c.date).ToList();*/ //Make sure the data is in ascending order of date.
            }

            CompaniesStocks companiesStocks = getCompaniesStocksModel(Stocks);
            return View(companiesStocks);
        }

        public IActionResult Compare(String[] symbols, String range, String parameter)
        {
            //Input: Comparing Parameter, Stocks to compare, Chart Range
            //Output: Symbols, Chart Data
            //String symbol = ViewData["symbol"].ToString();
            if (range == null)
            {
                range = "1m";
            }
            List<string> s = new List<string>();
            if (symbols.Length == 0)
            {
                s.Add("A");
                s.Add("AAPL");
                symbols = s.ToArray();
            }

            ViewData["range"] = range;
            ViewData["symbols"] = symbols;
            ViewData["param"] = parameter;
            ViewBag.dbSuccessChart = 0;
            //SaveCompanies();
            List<Stock> Stocks = null;
            SortedList<String, List<Stock>> companyStocks = new SortedList<String, List<Stock>>();
            if (symbols.Length != 0)
            {
                for (int i = 0; i < symbols.Length; i++)
                {
                    IEXHandler webHandler = new IEXHandler();
                    String symbol = symbols[i];
                    Stocks = webHandler.GetChart(symbols[i], range);
                    Stocks = Stocks.OrderBy(c => c.date).ToList(); //Make sure the data is in ascending order of date.
                    companyStocks.Add(symbol, Stocks);
                }
            }

            CompareCompanies compareCompanies = getCompareCompaniesModel(companyStocks);
            return View(compareCompanies);
            //return View();
        }

        public IActionResult Info(string symbol)
        {
            // Input: Stock Symbol
            // Output: Stock company information
            if(symbol == null)
            {
                symbol = "A";
            }
            CompanyInfo companyInfo = getCompanyInfo(symbol);
            return View(companyInfo);
        }

        private CompanyInfo getCompanyInfo(string symbol)
        {
            List<Company> companies = dbContext.Companies.ToList();

            IEXHandler webHandler = new IEXHandler();
            Information info = webHandler.GetInfo(symbol);
            if (info != null)
            {
                return(new CompanyInfo(companies,info));
            }
            else
            {
                return (new CompanyInfo(companies, null));
            }
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

        public IActionResult SaveCompanies()
        {
            List<Company> companies = new List<Company>();
            IEXHandler webHandler = new IEXHandler();
            companies = webHandler.GetSymbols();
            companies = companies.OrderBy(c => c.symbol).ToList();
            foreach (Company company in companies)
            {
                if (dbContext.Companies.Where(c => c.symbol.Equals(company.symbol)).Count() == 0)
                {
                    dbContext.Companies.Add(company);
                }
            }
            dbContext.SaveChanges();
            List<Stock> stocks = webHandler.GetChart("A", "1m");
            CompaniesStocks companiesStocks = getCompaniesStocksModel(stocks);
            ViewData["message"] = "Companies refreshed successfully.";
            return View("Index", companiesStocks);
        }

        /****
        * Returns the ViewModel CompaniesStocks based on the data provided.
        ****/
        public IActionResult SavePercentChanges()
        {
            List<Company> allCompanies = dbContext.Companies.OrderBy(c => c.symbol).ToList();
            List<CompanyChange> companyChange = new List<CompanyChange>();
            foreach (Company company in allCompanies.Take(50))
            {
                IEXHandler web = new IEXHandler();
                List<Stock> Stocks = web.GetTimeSeries(company.symbol);
                float averageChangePercentage = 0;
                foreach (Stock stock in Stocks)
                {
                    averageChangePercentage += stock.changePercent;
                }
                averageChangePercentage /= Stocks.Count;
                float change = (float)Math.Round(averageChangePercentage, 6);
                CompanyChange cc = new CompanyChange(company.symbol.ToString(), company.name.ToString(), change);
                if (dbContext.CompanyChanges.Where(c => c.Symbol.Equals(company.symbol)).Count() == 0)
                {
                    dbContext.CompanyChanges.AddAsync(cc);
                }
                else
                {
                    dbContext.CompanyChanges.Update(cc);
                }
                
            }
            
            dbContext.SaveChanges();
            IEXHandler webHandler = new IEXHandler();
            List<Stock> stocks = webHandler.GetChart("A", "1m");
            CompaniesStocks companiesStocks = getCompaniesStocksModel(stocks);
            ViewData["message"] = "Recommendation refreshed successfully.";
            return View("Index", companiesStocks);
        }

        public CompaniesStocks getCompaniesStocksModel(List<Stock> Stocks)
        {
            //SavePercentChanges();
            List<Company> companies = dbContext.Companies.ToList();
            List<CompanyChange> companyChanges = dbContext.CompanyChanges.ToList();
            companyChanges = companyChanges.OrderBy(c => c.AveragePercentChange).ToList();
            List<CompanyChange> top5 = companyChanges.TakeLast(5).ToList();
            List<CompanyChange> bottom5 = companyChanges.Take(5).ToList();
            if (Stocks.Count == 0)
            {
                return new CompaniesStocks(companies, null, null, null, "", "", "", "", "", "", "", "", "", 0, 0);
            }

            Stock current = Stocks.Last();
            string dates = string.Join(",", Stocks.Select(e => e.date));
            string highs = string.Join(",", Stocks.Select(e => e.high));
            string lows = string.Join(",", Stocks.Select(e => e.low));
            string closes = string.Join(",", Stocks.Select(e => e.close));
            string opens = string.Join(",", Stocks.Select(e => e.open));
            string changes = string.Join(",", Stocks.Select(e => e.change));
            string percentChanges = string.Join(",", Stocks.Select(e => e.changePercent));
            string changesovertime = string.Join(",", Stocks.Select(e => e.changeOverTime));
            string volumes = string.Join(",", Stocks.Select(e => e.volume / 1000000)); //Divide vol by million
            float avgprice = Stocks.Average(e => e.high);
            double avgvol = Stocks.Average(e => e.volume) / 1000000; //Divide volume by million
            return new CompaniesStocks(companies,top5, bottom5, Stocks.Last(), dates, opens, highs, lows, closes, volumes, changes, percentChanges, changesovertime, avgprice, avgvol);
        }

        public CompareCompanies getCompareCompaniesModel(SortedList<String, List<Stock>> companyStocks)
        {
            List<Company> companies = dbContext.Companies.ToList();
            //List<Company> compare = null;
            //List<Stock> compareStocks = null;
            //List<Stock> current = null;
            String dates = "";
            String opens = "";
            String highs = "";
            String lows = "";
            String closes = "";
            String changes = "";
            String percentChanges = "";
            String changesovertime = "";
            String volumes = "";
            float avgPrice = 0;
            Double avgVolume = 0;
            if (companyStocks.Count == 0)
            {
                return new CompareCompanies(companies, null);
            }
            List<CompareCompany> tempCompany = new List<CompareCompany>();
            for (int i = 0; i < companyStocks.Count; i++)
            {
                dates = string.Join(",", companyStocks.Values[i].Select(e => e.date));
                highs = string.Join(",", companyStocks.Values[i].Select(e => e.high));
                lows = string.Join(",", companyStocks.Values[i].Select(e => e.low));
                opens = string.Join(",", companyStocks.Values[i].Select(e => e.open));
                closes = string.Join(",", companyStocks.Values[i].Select(e => e.close));
                changes = string.Join(",", companyStocks.Values[i].Select(e => e.change));
                percentChanges = string.Join(",", companyStocks.Values[i].Select(e => e.changePercent));
                changesovertime = string.Join(",", companyStocks.Values[i].Select(e => e.changeOverTime));
                volumes = string.Join(",", companyStocks.Values[i].Select(e => e.volume / 1000000)); //Divide vol by million
                avgPrice = companyStocks.Values[i].Average(e => e.high);
                avgVolume = companyStocks.Values[i].Average(e => e.volume) / 1000000; //Divide volume by million
                tempCompany.Add(new CompareCompany(companyStocks.Values[i].Last(), companyStocks.Keys[i], dates, opens, lows, highs, closes, changes, percentChanges, changesovertime, volumes, avgPrice, avgVolume));
                //current.Add(companyStocks[i].Last());
                //dates.Add(string.Join(",", companyStocks[i].Select(e => e.date)));
                //prices.Add(string.Join(",", companyStocks[i].Select(e => e.high)));
                //volumes.Add(string.Join(",", companyStocks[i].Select(e => e.volume / 1000000))); //Divide vol by million
                //avgPrice.Add(companyStocks[i].Average(e => e.high));
                //avgVolume.Add(companyStocks[i].Average(e => e.volume) / 1000000); //Divide volume by million
            }
            return new CompareCompanies(companies, tempCompany);
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

            CompaniesStocks companiesStocks = getCompaniesStocksModel(stocks);
            ViewData["message"] = "Chart saved successfully for " + symbol + " for 1 month.";
            return View("Index", companiesStocks);
        }
    }
}
