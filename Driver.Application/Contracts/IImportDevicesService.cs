using Driver.Application.Model;

namespace Driver.Application.Contracts
{
    public interface IImportDevicesService
    {
        Task<ImportReportDto> ImportDevicesAsync();
    }
}
