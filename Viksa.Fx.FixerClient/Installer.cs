using Microsoft.Extensions.DependencyInjection;
using Viksa.Fx.Providers;

namespace Viksa.Fx.FixerClient
{
    public static class Installer
    {
        public static void InstallServices(IServiceCollection services)
        {            
            services.AddSingleton<IFxProvider, Client>();
        }
    }
}
