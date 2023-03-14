using Docusign.Repository.Peticion;
using Docusign.Services;
using Model.DTO.Users;

namespace API.Routes.MapDocusign
{
    public static class MapDocusignRoutes
    {
        public static void RegisterActividadEconomica(this IEndpointRouteBuilder app)
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
        }
    }
}
