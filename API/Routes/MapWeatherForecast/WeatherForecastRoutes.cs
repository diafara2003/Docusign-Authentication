
using Docusign.Repository.Peticion;


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

                return Results.Ok("bien");

            }).WithTags("WeatherForecast")
              .WithDescription("API de ejemplo para validar el funcionamiento del convertidor de pdf")
              .Produces<Stream>().WithOpenApi();

            app.MapGet("/WeatherForecast/pdf", async (IPeticionDocusignRepository service, string path) =>
            {

                var d = await service.FileToPDF(path);
                var mimeType = "application/pdf";
                return Results.File(d, contentType: mimeType);

                //return Results.Ok(path);

            }).WithTags("WeatherForecast")
               .WithDescription("API de ejemplo para validar el funcionamiento del convertidor de pdf")
               .Produces<Stream>().WithOpenApi();
        }

    }

}
