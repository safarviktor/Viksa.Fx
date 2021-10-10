using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Viksa.Fx.Providers;

namespace Viksa.Fx.FixerClient
{
    public class Client : IFxProvider
    {
        private const string ApiKey = "b157254cc88a3b02dfa1bd3314b0a906";        
        private const string ApiBaseUrl = "http://data.fixer.io/api/";
        private const string Base = "base";
        private const string Symbols = "symbols";

        private string AccessKeySyntax => $"access_key={ApiKey}";

        public async Task<decimal> GetFxRate(string fromCurrency, string toCurrency, DateTime date)
        {
            var jsonResult = await GetClient().GetStringAsync($"{date.ToString("yyyy-MM-dd")}?{AccessKeySyntax}&{Base}={fromCurrency}&{Symbols}={toCurrency}");
            var result = JsonConvert.DeserializeObject<GetRatesResponse>(jsonResult);
            return result.Rates.ContainsKey(toCurrency) ? result.Rates[toCurrency] : 0;
        }

        public async Task<decimal> GetLatestFxRate(string fromCurrency, string toCurrency)
        {
            var jsonResult = await GetClient().GetStringAsync($"latest?{AccessKeySyntax}&{Base}={fromCurrency}&{Symbols}={toCurrency}");
            var result = JsonConvert.DeserializeObject<GetRatesResponse>(jsonResult);
            return result.Rates.ContainsKey(toCurrency) ? result.Rates[toCurrency] : 0;
        }
        
        private HttpClient GetClient()
        {
            var client = HttpClientFactory.Create();
            client.BaseAddress = new Uri(ApiBaseUrl);
            return client;
        }
    }
}
