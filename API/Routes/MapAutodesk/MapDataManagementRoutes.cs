using Azure;
using Docusign.Services;
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
        var credentials = await Credentials.FromSessionAsync(_.Request.Cookies, _.Response.Cookies);
                if (credentials == null) { return null; }

                IList<jsTreeNode> nodes = new List<jsTreeNode>();

                /*
                // root
                List<jsTreeNode> _hubs = await new Proyectos().GetHubsAsync(Credentials.TokenInternal);
                List<jsTreeNode> _projects = await new Proyectos().GetProjectsAsync(_hubs.First().id, Credentials.TokenInternal);
                jsTreeNode _content_forlder = await new Proyectos().GetProjectContents(_hubs.First().id, _projects.First().id, Credentials.TokenInternal)
                    .ContinueWith(c => c.Result
                    .Where(c => c.text.Equals("Project Files"))
                    .FirstOrDefault());


                List<jsTreeNode> _files = await new Proyectos().GetFolderContents(_content_forlder.id, _projects.First().id, Credentials.TokenInternal);

                List<jsTreeNode> _rs = await new Proyectos().GetItemVersions(_files.FirstOrDefault().id, _projects.First().id, Credentials.TokenInternal);
                    return _rs;
                */
                // root
                string folder = ids.Split('/')[ids.Split('/').Length - 1];
                string projectId = ids.Split('/')[6];

                //  List<jsTreeNode> _file = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, "[items:autodesk.bim360:File]");

                List<jsTreeNode> _r = await new Proyectos().GetItemVersions(folder, projectId, credentials.TokenInternal);


                return Results.Ok( _r);
            }).WithTags("Docusign");
        }
    }
}
