using evat_workflow.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace evat_workflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;


        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.FromResult($"eVAT runing at url : {CustomSettings.Current.AppUrl}"));
        }
    }
}