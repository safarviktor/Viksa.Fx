using System;
using System.Threading.Tasks;
using Viksa.Fx.DataAccess;
using Viksa.Fx.Providers;

namespace Viksa.Fx.Business.Implementations
{
    internal class FxBusiness : IFxBusiness
    {
        private readonly IFxProvider _fxProvider;
        private readonly IFxRatesRepository _fxRatesRepository;

        public FxBusiness(IFxProvider fxProvider, IFxRatesRepository fxRatesRepository)
        {
            _fxProvider = fxProvider;
            _fxRatesRepository = fxRatesRepository;
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

        public async Task SaveLatestRates()
        {
            var latest = await _fxProvider.GetLatestAll();

            // get existing currencies
            // populate currencies we don't have
            // inspect and adjust currency unit (BTC comes as 0.000000214544 but other comes as 2135132184.54318)

            await _fxRatesRepository.Add(latest);
        }
    }
}
