using Docusign.Services;

namespace API.Routes.MapDocusign
{
    public static class Mapds
    {
        public static void RegisterDocusignDS(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ds/{id}/callback/", (HttpRequest request, IDocusignCallbackService _docusignService, IWebHostEnvironment _webHostEnvironment, string code) =>
            {
                string key = (request.RouteValues["id"] ?? "").ToString();

                _docusignService.SaveTokenFile(key, code, _webHostEnvironment.ContentRootPath);

                return Results.Ok("Se autenticó en DocuSign correctamente, puede cerrar esta ventana.");

            }).WithTags("Docusign")
               .WithDescription("callback de autenticacion")
               .Produces<string>().WithOpenApi();


            app.MapGet("/ds/callback/verificacion", (IDocusignCallbackService _docusignService, IWebHostEnvironment _webHostEnvironment, string code) =>
            {
                return Results.Ok(_docusignService.ReadTokenFile(_webHostEnvironment.ContentRootPath));
            }).WithTags("Docusign")
               .WithDescription("callback de autenticacion de verificacion")
               .Produces<string>().WithOpenApi();
        }
    }
}
