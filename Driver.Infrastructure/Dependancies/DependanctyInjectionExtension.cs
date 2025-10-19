using Devices.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Driver.Infrastructure.Dependancies
{
    public static class DependanctyInjectionExtension
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IExternalDeviceRepository, ExternalDataAccess>();
            services.AddScoped<IAppVariablesRootRepository, AppVisionDataAccess>();
        }
    }
}
