using System;
using System.Threading.Tasks;
using Viksa.Fx.Models;

namespace Viksa.Fx.Business
{
    public interface IFxBusiness
    {   
        Task<decimal> GetToAmount(decimal fromAmount, string fromCurrency, string toCurrency, DateTime? date = null);
        Task SaveLatestRates();
        Task<RateHistory> GetRateHistory(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate);
    }
}
