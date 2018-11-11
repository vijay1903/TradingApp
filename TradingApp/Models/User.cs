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
        public string Username { get; set; }
        public string Name { get; set; }
        public string DateJoined { get; set; }
        public string LastLogin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Stock> InterestedStocks { get; set; }
    }
}
