using Devices;
using Devices.Repositories;
using Driver.Infrastructure.OpcUa;

namespace Driver.Infrastructure
{
    internal class ExternalDataAccess : IExternalDeviceRepository
    {
        public async Task<IEnumerable<ExternalDevice>> GetExternalDevices()
        {
            var devices = new List<ExternalDevice>();

            devices.Add(new OpcUnite(1, Guid.NewGuid()));
            devices.Add(new OpcUnite(2, Guid.NewGuid()));

            devices.Add(new OpcSite(56, "007", new string[] { "val1", "val2", "val3" }, 2));
            devices.Add(new OpcSite(57, "00157", new string[] { "val4" }, 1));

            return devices;
        }
    }
}
