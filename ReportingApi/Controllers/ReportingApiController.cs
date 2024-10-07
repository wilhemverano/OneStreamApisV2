using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace ReportingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingApiController : ControllerBase
    {
        private readonly ILogger<ReportingApiController> _logger;

        public ReportingApiController(ILogger<ReportingApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("In ReportingApiController-Get");
                await Task.Delay(3000); // artificial delay
                return Ok("Response from ReportingApiController");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error processing the request.");
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Reporting value)
        {
            try
            {
                _logger.LogInformation("In ReportingApiController-Post");
                await Task.Delay(3000); // artificial delay
                return Ok($"ReportingApiController received: {value.ReportData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error processing the request.");
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
