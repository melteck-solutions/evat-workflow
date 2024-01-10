using Microsoft.AspNetCore.Mvc;

namespace evat_workflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HealthController> _logger;


        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "all")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}