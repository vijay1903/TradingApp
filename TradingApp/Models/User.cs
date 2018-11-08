using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class User
    {
        [Key]
        public string username { get; set; }
        public string name { get; set; }
        public string dateJoined { get; set; }
        public string lastLogin { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<Stock> interestedStocks { get; set; }
    }
}
