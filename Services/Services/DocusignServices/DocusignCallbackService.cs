
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Docusign.Services
{
    public interface IDocusignCallbackService
    {
        void SaveTokenFile(string code, string root);

        string GetNameFile();

        string ReadTokenFile(string folder);

        void DeleteTokenFile(string root);
    }

    public class DocusignCallbackService : IDocusignCallbackService
    {
        private IHttpContextAccessor httpContextAccessor;
        public DocusignCallbackService(IHttpContextAccessor _httpContextAccessor)
        {
            this.httpContextAccessor = _httpContextAccessor;
        }
        public string GetNameFile()
        {
            string rutaRequest = $"{httpContextAccessor.HttpContext.Request.PathBase.Value.Replace("/", "_").ToLower()}";

            return rutaRequest.ToLower();
        }

        public void SaveTokenFile(string code, string root)
        {
            string ruta = $"{root}\\token\\";


            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);


            ruta += $@"\{GetNameFile()}.txt";
            //Delete file token
            DeleteTokenFile(root);

            var archivo = File.Create(ruta);

            archivo.Close();


            using (StreamWriter file = new StreamWriter(ruta, true))
            {
                file.WriteLine(code);
                file.Close();
            }

        }


        public string ReadTokenFile(string folder)
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

            return texto;
        }

        public void DeleteTokenFile (string root)
        {
            string ruta = $"{root}\\token\\{GetNameFile()}.txt";
            if (File.Exists(ruta)) File.Delete(ruta);
        }
    }
}
