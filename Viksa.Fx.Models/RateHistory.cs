using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viksa.Fx.Models
{
    public class RateHistory
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public IEnumerable<RateWithDate> Rates { get; set; }
    }
}
