using Devices.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ValidationTests
{
    public class InMemoryMockedApp : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        public HttpClient Client { get; }

        public void Dispose()
        {
            _factory.Dispose();
            Client.Dispose();
        }

        public InMemoryMockedApp(Func<IExternalDeviceRepository>? externalDevicesRepo = null)
        {
            _factory = new WebApplicationFactory<Program>()
             .WithWebHostBuilder(builder =>
              {
                  builder.ConfigureServices(services =>
                  {
                      if (externalDevicesRepo != null)
                      {
                          DeleteRegisterService(services, typeof(IExternalDeviceRepository));
                          services.AddScoped<IExternalDeviceRepository>(_ => externalDevicesRepo());
                      }
                  });
              });

            Client = _factory.CreateClient();
        }

        private void DeleteRegisterService(IServiceCollection services, Type type)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == type);
            if (descriptor != null)
                services.Remove(descriptor);
        }
    }
}