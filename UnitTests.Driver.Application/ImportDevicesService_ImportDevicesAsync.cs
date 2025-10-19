using Devices;
using Devices.Repositories;
using Driver.Application;
using Driver.Application.Contracts;
using Driver.Application.Exceptions;
using Moq;
using UnitTests.Driver.Application.DomainMocks;

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
            var device = new Mock<ExternalDevice>();
            device.Setup(d => d.GetAppVariables()).Returns(new[] { variable });

            var devices = new List<ExternalDevice> { device.Object };

            _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync()).ReturnsAsync(new MockAppVariablesRoot_AddOrUpdate(true));
            _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices()).ReturnsAsync(devices);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UseCaseException>(() => _service.ImportDevicesAsync());

            Assert.Equal(nameof(ImportDevicesService.ImportDevicesAsync), exception.Source);
            Assert.IsType<ApplicationException>(exception.InnerException);
        }





        //[Fact]
        //public async Task ImportDevicesAsync_WithValidDevices_ShouldImportCorrectly()
        //{
        //    // Arrange
        //    var variable1 = new AppVariable(); // mock your variable if needed
        //    var variable2 = new AppVariable();

        //    var device1 = new Mock<ExternalDevice>();
        //    device1.Setup(d => d.GetAppVariables()).Returns(new[] { variable1 });

        //    var device2 = new Mock<ExternalDevice>();
        //    device2.Setup(d => d.GetAppVariables()).Returns(new[] { variable2 });

        //    var devices = new List<ExternalDevice> { device1.Object, device2.Object };

        //    var appVariablesRoot = new Mock<AppVariablesRoot>();
        //    appVariablesRoot.Setup(x => x.NewVariablesCount).Returns(1);
        //    appVariablesRoot.Setup(x => x.UpdatedVariablesCount).Returns(1);

        //    _appVariablesRepoMock.Setup(repo => repo.GetVariablesRootAsync())
        //        .ReturnsAsync(appVariablesRoot.Object);

        //    _externalDeviceRepoMock.Setup(repo => repo.GetExternalDevices())
        //        .ReturnsAsync(devices);

        //    // Act
        //    var result = await _service.ImportDevicesAsync();

        //    // Assert
        //    device1.Verify(d => d.GetAppVariables(), Times.Once);
        //    device2.Verify(d => d.GetAppVariables(), Times.Once);

        //    appVariablesRoot.Verify(a => a.AddOrUpdateVariable(variable1), Times.Once);
        //    appVariablesRoot.Verify(a => a.AddOrUpdateVariable(variable2), Times.Once);

        //    _appVariablesRepoMock.Verify(r => r.SaveAsync(appVariablesRoot.Object), Times.Once);

        //    Assert.Equal(2, result.TotalDevices);
        //    Assert.Equal(1, result.NewVariables);
        //    Assert.Equal(1, result.UpdatedVariables);
        //}



    }
}