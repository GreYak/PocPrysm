using Driver.Api.Controllers;
using Driver.Application.Contracts;
using Driver.Application.Exceptions;
using Driver.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Driver.Api
{
    public class AppVisionController_Get
    {
        private readonly Mock<IImportDevicesService> _importDevicesServiceMock;
        private readonly Mock<ILogger<AppVisionController>> _loggerMock;
        private readonly AppVisionController _controller;

        public AppVisionController_Get()
        {
            _importDevicesServiceMock = new Mock<IImportDevicesService>();
            _loggerMock = new Mock<ILogger<AppVisionController>>();
            _controller = new AppVisionController(_loggerMock.Object, _importDevicesServiceMock.Object);
        }

        [Fact]
        public async Task ImportDevicesIntoAppVision_WhenImportReportDto_ReturnsOk()
        {
            // Arrange
            var reportDto = new ImportReportDto(1, 2, 3);
            _importDevicesServiceMock
                .Setup(s => s.ImportDevicesAsync())
                .ReturnsAsync(reportDto);

            // Act
            var result = await _controller.ImportDevicesIntoAppVision();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(reportDto.Summary, okResult.Value);
        }

        [Fact]
        public async Task ImportDevicesIntoAppVision_WhenUseCaseExceptionThrown_ReturnsUnprocessableEntity()
        {
            // Arrange
            _importDevicesServiceMock
                .Setup(s => s.ImportDevicesAsync())
                .ThrowsAsync(new UseCaseException("importUseCase", new ApplicationException("Business rule violated.")));

            // Act
            var result = await _controller.ImportDevicesIntoAppVision();

            // Assert
            var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
            Assert.Equal(422, unprocessableResult.StatusCode);
        }

        [Fact]
        public async Task ImportDevicesIntoAppVision_WhenEntityNotFoundExceptionThrown_ReturnsNoContent()
        {
            // Arrange
            _importDevicesServiceMock
                .Setup(s => s.ImportDevicesAsync())
                .ThrowsAsync(new EntityNotFoundException("Device not found."));

            // Act
            var result = await _controller.ImportDevicesIntoAppVision();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ImportDevicesIntoAppVision_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _importDevicesServiceMock
                .Setup(s => s.ImportDevicesAsync())
                .ThrowsAsync(new Exception("Unexpected error."));

            // Act
            var result = await _controller.ImportDevicesIntoAppVision();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}