
using Docusign.Repository.Peticion;
using Microsoft.AspNetCore.Http;
using Model.DTO.Docusign;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Docusign.Services
{
    public interface IDocusignCallbackService
    {
        void SaveTokenFile(string key,string code, string root);

        string GetNameFile();

        DocusignAuthDTO ReadTokenFile(string folder);

        void DeleteTokenFile(string root);
    }

    public class DocusignCallbackService : IDocusignCallbackService
    {
        private IHttpContextAccessor httpContextAccessor;
        IPeticionDocusignAuth _peticionDocusignAuth;
        public DocusignCallbackService(IHttpContextAccessor _httpContextAccessor
            , IPeticionDocusignAuth _peticionDocusignAuth
            )
        {
            this.httpContextAccessor = _httpContextAccessor;
            this._peticionDocusignAuth = _peticionDocusignAuth;
        }
        public string GetNameFile()
        {
            string rutaRequest = $"{httpContextAccessor.HttpContext.Request.PathBase.Value.Replace("/", "_").ToLower()}";

            return rutaRequest.ToLower();
        }

        public async void SaveTokenFile(string key,string code, string root)
        {
            string name = GetNameFile();
            var access = await _peticionDocusignAuth.GetAccesToken(key,code);
            string ruta = $"{root}\\token\\";


            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);


            ruta += $@"\{name}.txt";
            //Delete file token
            string rutaDelete = $"{root}\\token\\{name}.txt";
            if (File.Exists(rutaDelete)) File.Delete(rutaDelete);

            var archivo = File.Create(ruta);

            archivo.Close();


            using (StreamWriter file = new StreamWriter(ruta, true))
            {
                file.WriteLine(JsonConvert.SerializeObject(access));
                file.Close();
            }

        }


        public DocusignAuthDTO ReadTokenFile(string folder)
        {
            string texto = string.Empty;
            try
            {
                using (var sr = new StreamReader($@"{folder}\token\\{GetNameFile()}.txt"))
                {
                    // Read the stream as a string, and write the string to the console.
                    texto = sr.ReadLine();

                    sr.Close();
                }
            }
            catch (System.Exception)
            {
                texto = string.Empty;
            }

            if (string.IsNullOrEmpty(texto)) return null;
            else
                return JsonConvert.DeserializeObject<DocusignAuthDTO>(texto);
        }

        public void DeleteTokenFile(string root)
        {
            string ruta = $"{root}\\token\\{GetNameFile()}.txt";
            if (File.Exists(ruta)) File.Delete(ruta);
        }
    }
}
