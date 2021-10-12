using System;
using System.Threading.Tasks;
using Viksa.Fx.Models;

namespace Viksa.Fx.DataAccess
{
    public interface IFxRatesRepository
    {
        Task Add(RatesByDate rates);
        Task<RateHistory> GetRateHistory(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate);
    }
}
