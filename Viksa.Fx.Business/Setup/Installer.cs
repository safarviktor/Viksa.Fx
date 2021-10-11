using Microsoft.Extensions.DependencyInjection;

namespace Viksa.Fx.Business.Setup
{
    public static class Installer
    {
        public static void InstallServices(IServiceCollection services)
        {
            Viksa.Fx.FixerClient.Installer.InstallServices(services);
            Viksa.Fx.DataAccess.Installer.InstallServices(services);

            services.AddTransient<IFxBusiness, Implementations.FxBusiness>();            
        }
    }
}
