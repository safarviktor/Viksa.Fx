using System;
using System.Collections.Generic;

namespace Viksa.Fx.Models
{
    public class RatesByDate
    {
        public DateTime Date { get; set; }
        public string FromCurrency { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
