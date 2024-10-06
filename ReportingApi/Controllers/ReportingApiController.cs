using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace ReportingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingApiController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(3000); // artificial delay
            return Ok("Response from ReportingApiController");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Reporting value)
        {
            await Task.Delay(3000); // artificial delay
            return Ok($"ReportingApiController received: {value.ReportData}");
        }
    }
}
