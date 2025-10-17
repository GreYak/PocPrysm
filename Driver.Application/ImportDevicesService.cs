using Devices;
using Devices.Repositories;
using Driver.Application.Contracts;
using Driver.Application.Model;

namespace Driver.Application
{
    internal class ImportDevicesService : IImportDevicesService
    {
        private IAppVariablesRootRepository _appVariablesRootRepository;
        private IExternalDeviceRepository _externalDeviceRepository;

        public ImportDevicesService(IAppVariablesRootRepository appVariablesRootRepository, IExternalDeviceRepository externalDeviceRepository)
        {
            _appVariablesRootRepository = appVariablesRootRepository;
            _externalDeviceRepository = externalDeviceRepository;
        }

        public async Task<ImportReportDto> ImportDevicesAsync()
        {
            AppVariablesRoot appVariablesRoot = await _appVariablesRootRepository.GetVariablesRootAsync();
            IEnumerable<ExternalDevice> externalDevices = await _externalDeviceRepository.GetExternalDevices();

            // TODO : try/catch
            foreach (ExternalDevice device in externalDevices)
            {
                foreach (AppVariable variableFromExternalDevice in device.GetAppVariables())
                    appVariablesRoot.AddOrUpdateVariable(variableFromExternalDevice);
            }

            await _appVariablesRootRepository.SaveAsync(appVariablesRoot);
            return new ImportReportDto(
                externalDevices.Count(),
                appVariablesRoot.NewVariablesCount,
                appVariablesRoot.UpdatedVariablesCount);
        }
    }
}
