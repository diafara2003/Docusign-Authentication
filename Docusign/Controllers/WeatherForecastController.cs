using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Docusign.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("docusign")]
        public IActionResult GetDocuSign() {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/WeatherForecast/userinfo" });
        }


        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {


            if(!User.Identity.IsAuthenticated) return Challenge(new AuthenticationProperties() { RedirectUri = "/WeatherForecast/userinfo" });

            HttpMessageHandler handler = new HttpClientHandler()
            {
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://na3.docusign.net/restapi/v2.1/accounts/56483961/users"),
                Timeout = new TimeSpan(0, 2, 0)
            };


            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");


            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + User.FindFirst(c => c.Type == "access_token")?.Value);

            HttpResponseMessage response = httpClient.GetAsync("https://na3.docusign.net/restapi/v2.1/accounts/56483961/users").Result;
            string content = string.Empty;

            using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
            {
                content = stream.ReadToEnd();
            }

            return Ok(content);
        }
    }
}
