using Driver.Application.Dependancies;
using Driver.Infrastructure.Dependancies;


namespace Driver.Api.Extensions
{
    internal static class DependanctyInjectionExtension
    {
        public static void RegisterStack(this IServiceCollection services)
        {
            // Logs
            services.AddLogging(builder => builder.AddConsole());

            // Layers
            services.RegisterApplicationServices();
            services.RegisterInfrastructureServices();
        }


    }
}
