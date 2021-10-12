using System;
using System.Threading.Tasks;
using Viksa.Fx.DataAccess;
using Viksa.Fx.Providers;
using System.Linq;
using System.Collections.Generic;
using Viksa.Fx.Models;

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
            await _fxRatesRepository.Add(latest);
        }

        public Task<RateHistory> GetRateHistory(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate)
        {
            return _fxRatesRepository.GetRateHistory(fromCurrency, toCurrency, fromDate, toDate);
        }
    }
}
