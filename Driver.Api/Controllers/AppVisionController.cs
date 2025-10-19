using Driver.Application.Contracts;
using Driver.Application.Exceptions;
using Driver.Application.Model;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppVisionController : ControllerBase
    {
        private readonly IImportDevicesService _importDevicesService;
        private readonly ILogger<AppVisionController> _logger;

        public AppVisionController(ILogger<AppVisionController> logger, IImportDevicesService importDevicesService)
        {
            _logger = logger;
            _importDevicesService = importDevicesService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportDevicesIntoAppVision()
        {
            var requestId = Guid.NewGuid();
            _logger.LogInformation($"New Http request for {nameof(ImportDevicesIntoAppVision)} with internal id : {requestId}");
            try
            {
                ImportReportDto importReport = await _importDevicesService.ImportDevicesAsync();
                _logger.LogInformation($"200 return for request {requestId}.");
                return Ok(importReport.Summary);
            }
            catch (UseCaseException exc)
            {
                _logger.LogInformation(exc, $"422 return  for request {requestId} because of {nameof(EntityNotFoundException)}");
                return UnprocessableEntity(exc.Message);
            }
            catch (EntityNotFoundException exc)
            {
                _logger.LogInformation(exc, $"204 return  for request {requestId} because of {nameof(EntityNotFoundException)}");
                return NoContent();
            }
            catch (Exception exc)
            {
                _logger.LogInformation(exc, $"500 return because of {nameof(Exception)}");
                return StatusCode(500, $"An internal error has been met  for request {requestId}. Retry later or contact yout administrator.");
            }
        }
    }
}