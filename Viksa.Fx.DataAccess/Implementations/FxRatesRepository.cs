using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Viksa.Fx.Models;

namespace Viksa.Fx.DataAccess.Implementations
{
    public class FxRatesRepository : BaseRepository, IFxRatesRepository
    {
        public Task Add(RatesByDate rates)
        {
            var ratesTable = new DataTable();
            ratesTable.Columns.Add("Key", typeof(string));
            ratesTable.Columns.Add("Value", typeof(decimal));

            foreach (var item in rates.Rates)
            {
                ratesTable.Rows.Add(item.Key, item.Value);
            }

            var @params = new DynamicParameters();
            @params.Add("AsAt", rates.Date);
            @params.Add("BaseCurrency", rates.FromCurrency);
            @params.Add("Rates", ratesTable.AsTableValuedParameter("dbo.KeyValuePairChar3Decimal"));

            return WithConnection(c => c.ExecuteAsync("dbo.spAddCurrencyRates", @params, commandType: System.Data.CommandType.StoredProcedure));                
        }

        public Task<RateHistory> GetRateHistory(string fromCurrency, string toCurrency, DateTime fromDate, DateTime toDate)
        {
            var sql = $@"
                DECLARE @fromId INT = (SELECT Id FROM dbo.Currency WHERE CurrencyCode = @{nameof(fromCurrency)})
                DECLARE @toId INT = (SELECT Id FROM dbo.Currency WHERE CurrencyCode = @{nameof(toCurrency)})

                SELECT 
	                AsAt AS {nameof(RateWithDate.Date)},
	                Rate AS {nameof(RateWithDate.Rate)}
                FROM dbo.CurrencyRate
                WHERE FromCurrencyId = @fromId
                AND ToCurrencyId = @toId
                AND AsAt BETWEEN @{nameof(fromDate)} AND @{nameof(toDate)} ";

            return WithConnection(async c =>
            {
                var dbRates = await c.QueryAsync<RateWithDate>(sql, new { fromCurrency, toCurrency, fromDate, toDate });

                return new RateHistory()
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Rates = dbRates
                };
            });
        }
    }
}
