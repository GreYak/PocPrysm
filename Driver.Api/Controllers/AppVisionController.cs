using Driver.Application.Contracts;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ImportReportDto importReport = await _importDevicesService.ImportDevicesAsync();
            return Ok(importReport.Summary);
        }
    }
}