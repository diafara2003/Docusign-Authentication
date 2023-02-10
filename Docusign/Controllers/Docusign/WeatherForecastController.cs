using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SincoSoft.Context;
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

        private IHttpContextAccessor _httpContextAccessor { get; }

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        //public WeatherForecastController(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

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
        public IActionResult GetDocuSign()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/WeatherForecast/userinfo" });
        }


        [HttpGet("url/ballback")]
        public IActionResult GetUserInfo()
        {
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            var path = _httpContextAccessor.HttpContext.Request.PathBase.Value;
            string callback = "";
            if (host.ToLower().Contains("localhost"))
            {
                callback = $"https://{host}{path}/ds/callback";
            }
            else
            {
                callback = $"https://{host}{path}/ds/callback";
            }

            return Ok(new
            {
                url = callback,
                urlReplace = callback.Replace("/", "%2F").Replace(":", "%3A")
            });
        }


    }
}
