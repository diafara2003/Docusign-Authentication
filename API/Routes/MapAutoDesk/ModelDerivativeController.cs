using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Autodesk.Forge;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using System.Linq;
using Repository.AutoDesk.forgeAPI;
using Model.DTO.Autodesk;

namespace ForgeAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModelDerivativeController : ControllerBase
    {
        private readonly ILogger<ModelDerivativeController> _logger;
        private Credentials Credentials { get; set; }

        public ModelDerivativeController(ILogger<ModelDerivativeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("folder/info")]
        public async Task<IActionResult> GetfolderInfo(string id) {

            string folder = id.Split('/')[id.Split('/').Length - 1];
            string projectId = id.Split('/')[6];

            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);

            FolferInfo folfer= await new Proyectos().Getfolfer(folder, projectId, Credentials.TokenInternal);

            List<jsTreeNode> content= await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, null);       

            return Ok(new Tuple<FolferInfo, List<jsTreeNode>>(folfer, content));
        }


        [HttpGet]
        [Route("folder/model/properties")]
        public async Task<IActionResult> GetPropertiesModelByFolder(string id)
        {
            DerivativesApi derivatives = new DerivativesApi();
            List<jsTreeNode> objlst = new List<jsTreeNode>();
            List<ExtraccionModeloRevitDTO> objresponse = new List<ExtraccionModeloRevitDTO>();

            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);
            if (Credentials == null) { return null; }


            string[] idParams = id.Split('/');
            string resource = idParams[idParams.Length - 2];
            switch (resource)
            {
                case "folders": // folders node selected/expanded, show folder contents

                    string folder = id.Split('/')[id.Split('/').Length - 1];
                    string projectId = id.Split('/')[6];

                    objlst = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, null);
                    break;
                //await new Proyectos().GetFolderContents(id, Credentials.TokenInternal);
                case "items":
                    objlst = await new Proyectos().GetItemVersions(id, Credentials.TokenInternal);
                    break;
            }

            foreach (var item in objlst)
            {
                if (!item.text.ToLower().Contains("3d") && item.text.ToLower().Contains(".rvt"))
                {
                    List<ExtraccionModeloRevitDTO> objResultado = await new ModelosRVT().ExtraerData(item.id, Credentials.TokenInternal, derivatives);

                    objResultado.ForEach(c => objresponse.Add(c));
                }
            }

            
            return Ok(objresponse.Take(10000));
        }

        //[HttpGet]7
        //[Route("model/properties")]
        //public async Task<IActionResult> GetPropertiesModel(string urn)
        //{

        //    Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);

        //    return Ok(await new ModelosRVT().ExtraerData(urn, Credentials.TokenInternal));
        //}

        [HttpGet]
        [Route("model/metadatas")]
        public async Task<string> GetMetadata(string id)
        {
            DerivativesApi derivatives = new DerivativesApi();


            string folder = id.Split('/')[id.Split('/').Length - 1];
            string projectId = id.Split('/')[6];

            List<jsTreeNode> _file = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, "[items:autodesk.bim360:File]");

            List<jsTreeNode> urn = await new Proyectos().GetItemVersions(_file.Last().id, projectId, Credentials.TokenInternal);



            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);

            derivatives.Configuration.AccessToken = Credentials.TokenInternal;


            dynamic meta = (await derivatives.GetMetadataAsync(urn.FirstOrDefault().id));

            return meta.data.metadata[0].guid;
        }

        [HttpGet]
        [Route("model/hierarchy")]
        public async Task<jsMetaData> GetHierarchy(string urn)
        {
            jsMetaData objresponse = new jsMetaData();
            DerivativesApi derivatives = new DerivativesApi();
            Credentials = await Credentials.FromSessionAsync(base.Request.Cookies, Response.Cookies);

            derivatives.Configuration.AccessToken = Credentials.TokenInternal;

            dynamic meta = (await derivatives.GetMetadataAsync(urn));

            string guid = meta.data.metadata[0].guid;

            dynamic properties = (await derivatives.GetModelviewPropertiesAsync(urn, guid)).ToJson();
            dynamic manifest = (await derivatives.GetModelviewMetadataAsync(urn, guid)).ToJson();


            objresponse.jstreenode = ConvertToActivity(manifest.data.objects[0].objects);
            objresponse.jsSingleNode = ConvertProperties(properties.data.collection); ;
            return objresponse;
        }
        List<jsTreeNode> ConvertProperties(dynamic res)
        {

            List<jsTreeNode> ovjResultado = new List<jsTreeNode>();

            ovjResultado = ((JArray)res).Select(x => new jsTreeNode
            {

                id = (string)x["objectid"],
                text = (string)x["name"],
            }).ToList();

            return ovjResultado;
        }

        List<jsTreeNodeB> ConvertToActivity(dynamic res)
        {
            List<jsTreeNodeB> ovjResultado = new List<jsTreeNodeB>();


            ovjResultado = ((JArray)res).
                Select(x => new jsTreeNodeB
                {
                    id = (string)x["objectid"],
                    text = (string)x["name"],
                    children = x["objects"] != null ? ConvertToActivity(x["objects"]) : null,
                }).ToList();

            return ovjResultado;

        }
    }
}
