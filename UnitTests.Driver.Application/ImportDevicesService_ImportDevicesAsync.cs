using Devices;
using Devices.Repositories;
using Driver.Application;
using Driver.Application.Contracts;
using Driver.Application.Exceptions;
using Driver.Application.Model;
using Moq;
using UnitTests.Driver.Application.DomainHelpers;

namespace UnitTests.Driver.Application
{
    public class ImportDevicesService_ImportDevicesAsync
    {
        private readonly Mock<IAppVariablesRootRepository> _appVariablesRepoMock;
        private readonly Mock<IExternalDeviceRepository> _externalDeviceRepoMock;
        private readonly IImportDevicesService _service;

        public ImportDevicesService_ImportDevicesAsync()
        {
            _appVariablesRepoMock = new Mock<IAppVariablesRootRepository>();
            _externalDeviceRepoMock = new Mock<IExternalDeviceRepository>();

            _service = new ImportDevicesService(
                _appVariablesRepoMock.Object,
                _externalDeviceRepoMock.Object);
        }

        [Fact]
        public async Task ImportDevicesAsync_WhenGetVariablesRootThrows_ShouldPropagateException()
        {
            // Arrange
            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync())
                .ThrowsAsync(new InvalidOperationException("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.ImportDevicesAsync());

            Assert.Equal("Unexpected error", exception.Message);
        }

        [Fact]
        public async Task ImportDevicesAsync_WhenGetExternalDevicesThrows_ShouldPropagateException()
        {
            // Arrange
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices())
                .ThrowsAsync(new TimeoutException("Timeout from external source"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
                _service.ImportDevicesAsync());

            Assert.Equal("Timeout from external source", exception.Message);
        }

        [Fact]
        public async Task ImportDevicesAsync_WhenNoDevicesFound_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(Enumerable.Empty<ExternalDevice>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.ImportDevicesAsync());
        }

        [Fact]
        public async Task ImportDevicesAsync_WhenApplicationExceptionOccurs_ShouldThrowUseCaseException()
        {
            // Arrange
            var variable = new AppVariable("myVar", "ExternalId_01", AppVariable.AppType.State);
            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync()).ReturnsAsync(new AppVariablesRootBuilder_AddOrUpdate(true).GenerateAppVariableRoot(variable, false));
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(new List<ExternalDevice> { new MockExternalDevices(variable) });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UseCaseException>(() => _service.ImportDevicesAsync());
        }

        [Fact]
        public async Task ImportDevicesAsync_WhenSaveAsyncThrown_ShouldPropagateException()
        {
            // Arrange
            var variable = new AppVariable("myVar", "ExternalId_01", AppVariable.AppType.State);
            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync()).ReturnsAsync(new AppVariablesRootBuilder_AddOrUpdate(false).GenerateAppVariableRoot(variable, false));
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(new List<ExternalDevice> { new MockExternalDevices(variable) });
            _appVariablesRepoMock.Setup(repo => repo.SaveAsync(It.IsAny<AppVariablesRoot>())).ThrowsAsync(new InvalidOperationException("DB error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ImportDevicesAsync());
            Assert.Equal("DB error", exception.Message);
        }

        [Fact]
        public async Task ImportDevicesAsync_WithNewValidDevices_ShouldImportCorrectly()
        {
            var variable = new AppVariable("myVar", "ExternalId_01", AppVariable.AppType.State);
            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync()).ReturnsAsync(new AppVariablesRootBuilder_AddOrUpdate(false).GenerateAppVariableRoot(variable, false));
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(new List<ExternalDevice> { new MockExternalDevices(variable) });
            _appVariablesRepoMock.Setup(repo => repo.SaveAsync(It.IsAny<AppVariablesRoot>())).Returns(Task.CompletedTask);

            // Act
            ImportReportDto importReportDtoResult = await _service.ImportDevicesAsync();

            // Assert
            Assert.NotNull(importReportDtoResult);
            Assert.Equal(1, importReportDtoResult.ExternalDevicesCount);
            Assert.Equal(1, importReportDtoResult.NewAppVariablesCount);
            Assert.Equal(0, importReportDtoResult.UpdatedAppVariablesCount);
        }

        [Fact]
        public async Task ImportDevicesAsync_WithValidDevicesForUpdate_ShouldImportCorrectly()
        {
            var variable = new AppVariable("myVar", "ExternalId_01", AppVariable.AppType.State);
            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync()).ReturnsAsync(new AppVariablesRootBuilder_AddOrUpdate(false).GenerateAppVariableRoot(variable, true));
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(new List<ExternalDevice> { new MockExternalDevices(variable) });
            _appVariablesRepoMock.Setup(repo => repo.SaveAsync(It.IsAny<AppVariablesRoot>())).Returns(Task.CompletedTask);

            // Act
            ImportReportDto importReportDtoResult = await _service.ImportDevicesAsync();

            // Assert
            Assert.NotNull(importReportDtoResult);
            Assert.Equal(1, importReportDtoResult.ExternalDevicesCount);
            Assert.Equal(0, importReportDtoResult.NewAppVariablesCount);
            Assert.Equal(1, importReportDtoResult.UpdatedAppVariablesCount);
        }
    }
}