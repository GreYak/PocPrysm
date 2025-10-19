using Driver.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Driver.Application.Dependancies
{
    public static class DependanctyInjectionExtension
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IImportDevicesService, ImportDevicesService>();
        }
    }
}
