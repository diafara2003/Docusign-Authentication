using Autodesk.Forge;
using Autodesk.Forge.Model;
using Microsoft.AspNetCore.Http;
using Model.DTO.Autodesk;
using Newtonsoft.Json.Linq;
using Repository.AutoDesk.forgeAPI;
using Services.BIM360Services;
using System.Net.Http;

namespace API.Routes.MapAutodesk
{
    public static class MapModelDerivativeRoutes
    {
        private static Credentials Credentials { get; set; }
        public static void RegisterModelDerivative(this IEndpointRouteBuilder app)
        {
            app.MapGet("/edt/BIM360/ModelDerivative/folder/info", async (HttpContext _httpContext, string ids) =>
            {
                string folder = ids.Split('/')[ids.Split('/').Length - 1];
                string projectId = ids.Split('/')[6];

                Credentials = await Credentials.FromSessionAsync(_httpContext.Request.Cookies, _httpContext.Response.Cookies);

                FolferInfo folfer = await new Proyectos().Getfolfer(folder, projectId, Credentials.TokenInternal);

                List<jsTreeNode> content = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, null);


                return Results.Ok(new Tuple<FolferInfo, List<jsTreeNode>>(folfer, content));
            }).WithTags("AutoDesk");

            app.MapGet("/EDT/BIM360/ModelDerivative/folder/model/properties", async (IBIM360Services bim, HttpContext _httpContext, string id) =>
            {
                DerivativesApi derivatives = new DerivativesApi();
                List<jsTreeNode> objlst = new List<jsTreeNode>();
                List<MigracionRevitDTO> objresponse = new List<MigracionRevitDTO>();

                Credentials = await Credentials.FromSessionAsync(_httpContext.Request.Cookies, _httpContext.Response.Cookies);
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
                    //await new Proyectos().GetFolderContents(id, _Credentials.TokenInternal);
                    case "items":
                        objlst = await new Proyectos().GetItemVersions(id, Credentials.TokenInternal);
                        break;
                }

                foreach (var item in objlst)
                {
                    if (!item.text.ToLower().Contains("3d") && item.text.ToLower().Contains(".rvt"))
                    {
                        List<MigracionRevitDTO> objResultado = await new ModelosRVT().ExtraerData(item.id, Credentials.TokenInternal, derivatives);

                        objResultado.ForEach(c => objresponse.Add(c));
                    }
                }
                var _res = bim.MigracionRevit(objresponse.ToList());
                return Results.Ok(new
                {

                    datos = objresponse.Take(500),
                    resultado = _res,
                    totalregistros= objresponse.Count
                });
            }).WithTags("AutoDesk");

            app.MapGet("/EDT/BIM360/ModelDerivative/model/metadatas", async (HttpContext _httpContext, string id) =>
            {
                DerivativesApi derivatives = new DerivativesApi();


                string folder = id.Split('/')[id.Split('/').Length - 1];
                string projectId = id.Split('/')[6];

                List<jsTreeNode> _file = await new Proyectos().GetFolderContents(folder, projectId, Credentials.TokenInternal, "[items:autodesk.bim360:File]");

                List<jsTreeNode> urn = await new Proyectos().GetItemVersions(_file.Last().id, projectId, Credentials.TokenInternal);

                Credentials = await Credentials.FromSessionAsync(_httpContext.Request.Cookies, _httpContext.Response.Cookies);

                derivatives.Configuration.AccessToken = Credentials.TokenInternal;

                dynamic meta = (await derivatives.GetMetadataAsync(urn.FirstOrDefault().id));

                return Results.Ok(meta.data.metadata[0].guid);
            }).WithTags("AutoDesk");

            app.MapGet("/EDT/BIM360/ModelDerivative/model/hierarchy", async (HttpContext _httpContext, string urn) =>
            {
                jsMetaData objresponse = new jsMetaData();
                DerivativesApi derivatives = new DerivativesApi();
                Credentials = await Credentials.FromSessionAsync(_httpContext.Request.Cookies, _httpContext.Response.Cookies);

                derivatives.Configuration.AccessToken = Credentials.TokenInternal;

                dynamic meta = (await derivatives.GetMetadataAsync(urn));

                string guid = meta.data.metadata[0].guid;

                dynamic properties = (await derivatives.GetModelviewPropertiesAsync(urn, guid)).ToJson();
                dynamic manifest = (await derivatives.GetModelviewMetadataAsync(urn, guid)).ToJson();


                objresponse.jstreenode = ConvertToActivity(manifest.data.objects[0].objects);
                objresponse.jsSingleNode = ConvertProperties(properties.data.collection);

                return Results.Ok(objresponse);
            }).WithTags("AutoDesk");


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
}
