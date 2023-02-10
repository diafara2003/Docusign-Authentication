using Microsoft.AspNetCore.Mvc;
using Model.DTO.Autodesk;
using Repository.AutoDesk.forgeAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_CORE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {

        private Credentials Credentials { get; set; }


        [HttpGet]
        public async Task<List<jsTreeNode>> GetProjectsAsync(string id)
        {
            List<jsTreeNode> objresponse = new List<jsTreeNode>();
            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);
            if (Credentials == null) { return null; }


            if (id.Split("proyecto_").Length == 2) // root
                return await new Proyectos().GetHubsAsync(Credentials.TokenInternal, id.Split("proyecto_")[1]);

            string[] idParams = id.Split('/');
            string resource = idParams[idParams.Length - 2];
            switch (resource)
            {
                case "hubs": // hubs node selected/expanded, show projects
                    return await new Proyectos().GetProjectsAsync(id, Credentials.TokenInternal);
                case "projects": // projects node selected/expanded, show root folder contents
                    return await new Proyectos().GetProjectContents(id, Credentials.TokenInternal);
                case "folders": // folders node selected/expanded, show folder contents

                    string folder = id.Split('/')[id.Split('/').Length - 1];
                    string projectId = id.Split('/')[6];

                    return await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, null);

                //await new Proyectos().GetFolderContents(id, Credentials.TokenInternal);
                case "items":
                    return await new Proyectos().GetItemVersions(id, Credentials.TokenInternal);
            }

            return objresponse;
        }

    }
}
