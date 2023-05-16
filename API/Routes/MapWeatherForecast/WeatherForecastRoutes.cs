
using Autodesk.Forge.Model;
using Azure;
using Azure.Core;
using Docusign.Repository.Peticion;
using Docusign.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web;

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
            app.MapGet("/WeatherForecast", () =>
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


            


            app.MapGet("/WeatherForecast/pdf", async (IPeticionDocusignRepository service, string path) =>
            {
                
                var d=  await service.FileToPDF(path);
                var mimeType = "application/pdf";
                return Results.File(d, contentType: mimeType);

            }).WithTags("WeatherForecast");
        }

    }

}
