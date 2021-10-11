using System;
using System.Threading.Tasks;

namespace Viksa.Fx.Business
{
    public interface IFxBusiness
    {   
        Task<decimal> GetToAmount(decimal fromAmount, string fromCurrency, string toCurrency, DateTime? date = null);
        Task SaveLatestRates();
    }
}
