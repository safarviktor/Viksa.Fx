using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Viksa.Fx.Business;

namespace Viksa.Fx.ConsoleDaily
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Viksa.Fx.Business.Setup.Installer.InstallServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var fxProvider = serviceProvider.GetService<IFxBusiness>();

            return fxProvider.SaveLatestRates();
        }
    }
}
