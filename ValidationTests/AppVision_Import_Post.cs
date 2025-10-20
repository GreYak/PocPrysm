using Devices.Repositories;
using Moq;
using System.Net;

namespace ValidationTests
{
    public class AppVision_Import_Post
    {
        private const string _url = "appvision/import";

        [Fact]
        public async Task ImportDevices_NominalCase()
        {
            // Arrange 
            using var app = new InMemoryMockedApp();

            // Act
            var response = await app.Client.PostAsync(_url, null);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("4 devices have been found to create 6 variables into AppVision and to update 0 others.", content);
        }

        [Fact]
        public async Task ImportDevices_NoDevicesCase()
        {
            // Arrange 
            var mockExternalDevicesRepo = new Mock<IExternalDeviceRepository>();
            using var app = new InMemoryMockedApp(() => mockExternalDevicesRepo.Object);

            // Act
            var response = await app.Client.PostAsync(_url, null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(string.Empty, content);
        }

        // TODO : add + update
        // TODO : check base
    }
}
