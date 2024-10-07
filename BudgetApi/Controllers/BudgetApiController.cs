using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetApiController : ControllerBase
    {
        private readonly ILogger<BudgetApiController> _logger;

        public BudgetApiController(ILogger<BudgetApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {                
                _logger.LogInformation("In BudgetApiController-Get");
                await Task.Delay(3000); //  artificial delay
                return Ok("Response from BudgetApiController");
            }
            catch (Exception ex)
            {
                _logger.LogError("There was an error processing the request.", ex);
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Budget value)
        {
            try
            {
                // mock exception
                if (string.IsNullOrWhiteSpace(value.BudgetData))
                {
                    throw new Exception("Error, cannot get budget with no data");
                }
                _logger.LogInformation("In BudgetApiController-Post");
                await Task.Delay(3000); // artificial delay
                return Ok($"BudgetApiController received: {value.BudgetData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error processing the request.");
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
