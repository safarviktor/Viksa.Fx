using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Viksa.Fx.Business;

namespace Viksa.Fx.Console
{
    class Program
    {
        private const string DateFormat = "yyyy-MM-dd";

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Viksa.Fx.Business.Setup.Installer.InstallServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var fxProvider = serviceProvider.GetService<IFxBusiness>();
            
            while(true)
            {
                System.Console.WriteLine("WELCOME! This app will convert an amount between two currencies.");

                var fromCurrency = GetFromCurrency();
                var fromAmount = GetFromAmount();
                var toCurrency = GetToCurrency();
                var date = GetDate();

                var toAmount = await fxProvider.GetToAmount(fromAmount, fromCurrency, toCurrency, date);
                var dateSuffix = date.HasValue ? $" as at {date.Value.ToString(DateFormat)}" : string.Empty;

                System.Console.WriteLine($"{fromAmount} {fromCurrency} = {toAmount} {toCurrency}{dateSuffix}");
                System.Console.WriteLine("FX rate provided by fixer.io.");
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
        }

        private static DateTime? GetDate()
        {
            System.Console.Write($"Enter conversion date ({DateFormat}) [leave empty for latest]:");
            var inputDate = System.Console.ReadLine();
            if (string.IsNullOrEmpty(inputDate))
            {
                return null;
            }

            return DateTime.ParseExact(inputDate, DateFormat, CultureInfo.InvariantCulture);
        }

        private static string GetToCurrency()
        {
            System.Console.Write("Enter to-currency [default=USD]:");
            var toCurrency = System.Console.ReadLine();
            if (string.IsNullOrEmpty(toCurrency))
            {
                toCurrency = "USD";
            }
            return toCurrency;
        }

        private static decimal GetFromAmount()
        {
            string input = "nonParseableDecimal";
            decimal fromAmount = 0;
            while (!decimal.TryParse(input, out fromAmount))
            {
                System.Console.Write("Enter amount:");
                input = System.Console.ReadLine(); 
            }

            return fromAmount;
        }

        private static string GetFromCurrency()
        {
            System.Console.Write("Enter from-currency [default=EUR]:");
            var fromCurrency = System.Console.ReadLine();
            if (string.IsNullOrEmpty(fromCurrency))
            {
                fromCurrency = "EUR";
            }

            return fromCurrency;
        }
    }
}
