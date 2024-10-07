using CPMFrontendApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CPMFrontendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPMFrontendApiController : ControllerBase
    {
        private readonly IService _service;
        private readonly ILogger<CPMFrontendApiController> _logger;

        public CPMFrontendApiController(IService service, ILogger<CPMFrontendApiController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("In CPMFrontendApiController-Get");
                var response = await _service.GetFinanceProcessesDataAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("There was an error processing the request.", ex);
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CombinedModel value)
        {
            try
            {
                _logger.LogInformation("In CPMFrontendApiController-Post");
                var response = await _service.PostFinanceProcessesDataAsync(value);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error processing the request.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
