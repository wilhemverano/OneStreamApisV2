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

        public CPMFrontendApiController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GetFinanceProcessesDataAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CombinedModel value)
        {
            var response = await _service.PostFinanceProcessesDataAsync(value);
            return Ok(response);
        }
    }
}
