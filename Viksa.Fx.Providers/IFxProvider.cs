using System;
using System.Threading.Tasks;
using Viksa.Fx.Models;

namespace Viksa.Fx.Providers
{
    public interface IFxProvider
    {
        Task<decimal> GetLatestFxRate(string fromCurrency, string toCurrency);
        Task<decimal> GetFxRate(string fromCurrency, string toCurrency, DateTime date);
        Task<RatesByDate> GetLatestAll();
    }
}
