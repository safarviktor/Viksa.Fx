using System;
using System.Threading.Tasks;

namespace Viksa.Fx.Providers
{
    public interface IFxProvider
    {
        Task<decimal> GetLatestFxRate(string fromCurrency, string toCurrency);
        Task<decimal> GetFxRate(string fromCurrency, string toCurrency, DateTime date);
    }
}
