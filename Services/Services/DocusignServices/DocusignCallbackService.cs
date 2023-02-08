﻿
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Docusign.Services
{
    public interface IDocusignCallbackService
    {
        void SaveTokenFile(string code, string root);

        string GetNameFile();
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
            string rutaRequest = $"{httpContextAccessor.HttpContext.Request.PathBase.Value.Replace("/", "_").ToLower()}_{httpContextAccessor.HttpContext.Request.Path.Value.Replace("/", "_").ToLower()}";

            return rutaRequest.ToLower();
        }

        public void SaveTokenFile(string code, string root)
        {
            string ruta = $"{root}\\token\\";


            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);


            ruta += $@"\{GetNameFile()}.txt";

            if (File.Exists(ruta)) File.Delete(ruta);


            var archivo = File.Create(ruta);
            archivo.Close();

            using (StreamWriter file = new StreamWriter(ruta, true))
            {
                file.WriteLine(code);
                file.Close();
            }

        }
    }
}
