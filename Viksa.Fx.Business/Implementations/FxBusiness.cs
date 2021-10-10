using System;
using System.Threading.Tasks;
using Viksa.Fx.Providers;

namespace Viksa.Fx.Business.Implementations
{
    internal class FxBusiness : IFxBusiness
    {
        private readonly IFxProvider _fxProvider;

        public FxBusiness(IFxProvider fxProvider)
        {
            _fxProvider = fxProvider;
        }

        public async Task<decimal> GetToAmount(decimal fromAmount, string fromCurrency, string toCurrency, DateTime? date = null)
        {
            decimal fxRate = 0;

            if (date.HasValue)
            {
                fxRate = await _fxProvider.GetFxRate(fromCurrency, toCurrency, date.Value);                
            }
            else
            {
                fxRate = await _fxProvider.GetLatestFxRate(fromCurrency, toCurrency);
            }

            return fxRate * fromAmount;
        }
    }
}
