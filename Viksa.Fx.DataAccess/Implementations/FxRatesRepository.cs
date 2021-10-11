using Dapper;
using System.Data;
using System.Data.SqlClient;
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
    }
}
