using Docusign.Repository.Peticion;
using Docusign.Services;
using Model.DTO.Users;

namespace API.Routes.MapDocusign
{
    public static class MapDocusignRoutes
    {
        public static void RegisterDocusign(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Docusign/userInfo",async  (IDocusignService _docusignService) =>
            {
                try
                {
                    userDTO _user = await _docusignService.peticion<userDTO>("users", MethodRequest.GET);
                    return Results.Ok(_user);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/api/Docusign/Templates", async (IDocusignService _docusignService) =>
            {
                try
                {
                    var _result = await _docusignService.GetTemplates(); ;
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/api/Docusign/TemplatesSigners", async (IDocusignService _docusignService) =>
            {
                try
                {
                    var _result = await _docusignService.GetTemplatesSigners(); 
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/api/Docusign/envelopes/recipents", async (IDocusignService _docusignService) =>
            {
                try
                {
                    var _result = await _docusignService.GetTemplatesSigners();
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");
        }
    }
}
