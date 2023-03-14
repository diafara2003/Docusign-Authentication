using Model.DTO.Autodesk;
using Repository.AutoDesk.forgeAPI;

namespace API.Routes.MapAutoDesk
{
    public static class MapProyectosRoutes
    {
        private static Credentials Credentials { get; set; }
        public static void RegisterProyectos(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Proyectos", async (HttpContext _httpContext, string id) =>
            {
                List<jsTreeNode> objresponse = new List<jsTreeNode>();
                Credentials = await Credentials.FromSessionAsync(_httpContext.Request.Cookies, _httpContext.Response.Cookies);
                if (Credentials == null) { return Results.Ok(null); }


                if (id.Split("proyecto_").Length == 2) // root
                    return Results.Ok(await new Proyectos().GetHubsAsync(Credentials.TokenInternal, id.Split("proyecto_")[1]));

                string[] idParams = id.Split('/');
                string resource = idParams[idParams.Length - 2];
                switch (resource)
                {
                    case "hubs": // hubs node selected/expanded, show projects
                        return Results.Ok(await new Proyectos().GetProjectsAsync(id, Credentials.TokenInternal));
                    case "projects": // projects node selected/expanded, show root folder contents
                        return Results.Ok(await new Proyectos().GetProjectContents(id, Credentials.TokenInternal));
                    case "folders": // folders node selected/expanded, show folder contents

                        string folder = id.Split('/')[id.Split('/').Length - 1];
                        string projectId = id.Split('/')[6];

                        return Results.Ok(await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, null));

                    //await new Proyectos().GetFolderContents(id, Credentials.TokenInternal);
                    case "items":
                        return Results.Ok(await new Proyectos().GetItemVersions(id, Credentials.TokenInternal));
                }
                return Results.Ok(objresponse);
            }).WithTags("AutoDesk");
        }
    }
}
