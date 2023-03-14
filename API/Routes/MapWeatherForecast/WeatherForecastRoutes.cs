﻿
using Docusign.Services;


namespace API.Routes.MapWeatherForecast
{
    public static class WeatherForecastRoutes
    {

        private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public static void RegisterWeatherForecast(this IEndpointRouteBuilder app)
        {
            app.MapGet("/WeatherForecast",  () =>
            {
                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            }).WithTags("WeatherForecast");
        }
    }

}
