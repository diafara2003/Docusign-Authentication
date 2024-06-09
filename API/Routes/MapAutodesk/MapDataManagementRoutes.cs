using Model.DTO.Autodesk;
using Repository.AutoDesk.forgeAPI;

namespace API.Routes.MapAutodesk
{
    public static class MapDataManagementRoutes
    {
        public static void RegisterDataManagement(this IEndpointRouteBuilder app)
        {
            app.MapGet("/EDT/BIM360/DataManagement", async (HttpContext _, string ids) =>
            {
                try
                {
                    var credentials = await Credentials.FromSessionAsync(_.Request.Cookies, _.Response.Cookies);
                    if (credentials == null) { return null; }

                    IList<jsTreeNode> nodes = new List<jsTreeNode>();


                    string folder = ids.Split('/')[ids.Split('/').Length - 1];
                    string projectId = ids.Split('/')[6];

                    //  List<jsTreeNode> _file = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, "[items:autodesk.bim360:File]");

                    List<jsTreeNode> _r = await new Proyectos().GetItemVersions(folder, projectId, credentials.TokenInternal);


                    return Results.Ok(_r);
                }
                catch (Exception e)
                {

                    throw e;
                }
   
            }).WithTags("BIM360");
        }
    }
}
