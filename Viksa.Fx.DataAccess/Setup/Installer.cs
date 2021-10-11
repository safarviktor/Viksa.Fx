using Microsoft.Extensions.DependencyInjection;

namespace Viksa.Fx.DataAccess
{
    public static class Installer
    {
        public static void InstallServices(IServiceCollection services)
        {
            services.AddTransient<IFxRatesRepository, Implementations.FxRatesRepository>();
        }
    }
}
