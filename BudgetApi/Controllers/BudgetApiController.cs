using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetApiController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(3000); //  artificial delay
            return Ok("Response from BudgetApiController");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Budget value)
        {
            await Task.Delay(3000); // artificial delay
            return Ok($"BudgetApiController received: {value.BudgetData}");
        }
    }
}
