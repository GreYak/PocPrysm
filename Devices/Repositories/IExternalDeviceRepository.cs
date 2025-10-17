
namespace Devices.Repositories
{
    public interface IExternalDeviceRepository
    {
        Task<IEnumerable<ExternalDevice>> GetExternalDevices();
    }
}
