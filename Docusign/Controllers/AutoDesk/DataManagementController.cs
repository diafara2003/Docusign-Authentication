using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Autodesk.Forge.Model;
using Microsoft.Extensions.Logging;
using Model.DTO.Autodesk;
using Repository.AutoDesk.forgeAPI;

namespace ForgeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataManagementController : ControllerBase
    {
        private readonly ILogger<DataManagementController> _logger;
        private Credentials Credentials { get; set; }

        public DataManagementController(ILogger<DataManagementController> logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// GET TreeNode passing the ID
        /// </summary>
        [HttpGet]
        //[Route("datamanagement")]
        public async Task<IList<jsTreeNode>> GetTreeNodeAsync(string id = "")
        {
            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);
            if (Credentials == null) { return null; }

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
            string folder = id.Split('/')[id.Split('/').Length - 1];
            string projectId = id.Split('/')[6];

            //  List<jsTreeNode> _file = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, "[items:autodesk.bim360:File]");

            List<jsTreeNode> _r = await new Proyectos().GetItemVersions(folder, projectId, Credentials.TokenInternal);

          
            return _r;

        }

        private string GetName(DynamicDictionaryItems folderIncluded, KeyValuePair<string, dynamic> folderContentItem)
        {
            return "N/A";
        }

    }
}
