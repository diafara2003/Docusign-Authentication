using Autodesk.Forge;
using Model.DTO.Autodesk;
using Repository.AutoDesk.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.AutoDesk.forgeAPI
{
    public class ModelosRVT
    {

        public async Task<List<MigracionRevitDTO>> ExtraerData(string urn, string TokenInternal, DerivativesApi derivatives)
        {

            derivatives.Configuration.AccessToken = TokenInternal;

            List<jsTreeNode> version_document = await new Proyectos().GetItemVersions(urn, TokenInternal);

            var maniffest = await derivatives.GetManifestAsync(version_document.FirstOrDefault().id);

            dynamic meta = (await derivatives.GetMetadataAsync(version_document.FirstOrDefault().id));

            string guid = meta.data.metadata[0].guid;
            string name = string.Format("{0}_{1}", meta.data.metadata[1].name, maniffest.derivatives[0].name);


            dynamic d = await derivatives.GetModelviewMetadataAsync(version_document.FirstOrDefault().id, guid);

            dynamic properties = ( derivatives.GetModelviewProperties(version_document.FirstOrDefault().id, guid));
        

            
            if (properties.data != null && properties.data.collection != null)
                return MapExtraccionModeloRevit.Map(properties.data.collection, name, guid, version_document.FirstOrDefault().id, d);
            else 
            return new List<MigracionRevitDTO>();

        }





    }
}
