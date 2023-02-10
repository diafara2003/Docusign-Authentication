

using Model.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Docusign.Repository.DataBase.Conexion;
using Microsoft.AspNetCore.Http;

namespace Docusign.Repository.Peticion
{
    public enum MethodRequest
    {
        GET,
        POST
    }

    public interface IPeticionDocusignRepository
    {
        Task<T> peticion<T>(string method, MethodRequest type, object data = null);

        Task<T> peticionFile<T>(string method, object data = null);

        AuthenticationDTO validationAuthentication();


    }

    public class PeticionDocusignRepository : IPeticionDocusignRepository
    {
        private DB_ADPRO contexto;
        private IHttpContextAccessor httpContextAccessor;
        public PeticionDocusignRepository(DB_ADPRO _contexto, IHttpContextAccessor _httpContextAccessor)
        {
            this.contexto = _contexto;
            this.httpContextAccessor = _httpContextAccessor;
        }

        //public PeticionDocusignRepository(IConstruirSession construirSession, IHttpContextAccessor httpContextAccessor)
        //{
        //    // this.contexto = _contexto;
        //}

        public async Task<T> peticion<T>(string method, MethodRequest type, object data = null)
        {

            HttpMessageHandler handler = new HttpClientHandler();

            var token = contexto.tokenDocusign.Take(1).First();
            string account = contexto.adpconfig.FirstOrDefault(c => c.CnfCodigo == "CuentaDocuSign").CnfValor;

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/{account}/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };


            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);

            HttpResponseMessage response = new HttpResponseMessage();

            if (type == MethodRequest.GET)
            {
                response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/{account}/{method}");
            }
            else if (type == MethodRequest.POST)
            {
                var json = JsonConvert.SerializeObject(data);
                var dataEnvio = new StringContent(json, Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync($"https://na3.docusign.net/restapi/v2.1/accounts/{account}/{method}", dataEnvio);
            }


            string content = string.Empty;

            using (StreamReader stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                content = stream.ReadToEnd();
            }

            var x = JsonConvert.DeserializeObject<T>(content);

            return x;
        }

        public async Task<T> peticionFile<T>(string method, object data = null)
        {

            string account = contexto.adpconfig.FirstOrDefault(c => c.CnfCodigo == "CuentaDocuSign").CnfValor;
            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/{account}/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            var token = contexto.tokenDocusign.Take(1).First();



            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);

            HttpResponseMessage response = new HttpResponseMessage();

            response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/{account}/{method}");

            byte[] content = await response.Content.ReadAsByteArrayAsync();

            string fileBase64 = Convert.ToBase64String(content);

            fileBase64 = "{\"documentBase64\" :\"" + fileBase64 + "\"}";

            var x = JsonConvert.DeserializeObject<T>(fileBase64);

            return x;
        }

        public AuthenticationDTO validationAuthentication()
        {
            AuthenticationDTO auth = new AuthenticationDTO();


            var host = httpContextAccessor.HttpContext.Request.Host.Value;
            var path = httpContextAccessor.HttpContext.Request.PathBase.Value;

            string callback = $"https://{host}{path}/api/ds/callback".Replace("/", "%2F").Replace(":", "%3A");

            string client_id = contexto.adpconfig.Where(c => c.CnfCodigo == "Client_id_docusign").FirstOrDefault().CnfValor;
            var token = contexto.tokenDocusign.ToList();
            string url = $"https://account.docusign.com/oauth/auth?client_id={client_id}&scope=signature&response_type=code&redirect_uri={callback}";


            if (token.Count == 0)
            {
                auth.isAuthenticated = false;
                auth.URL = url;


                contexto.SaveChanges();

                return auth;
            }
            
            
            //if (token.FirstOrDefault().Fecha <= DateTime.Now.AddHours(8))
                if (DateTime.Now <= token.FirstOrDefault().Fecha.AddHours(8))
                {
                auth.isAuthenticated = true;
                auth.URL = url;
                return auth;
            }
            else
            {
                auth.isAuthenticated = false;
                auth.URL = url;

                contexto.Entry(token.FirstOrDefault()).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                contexto.SaveChanges();

                return auth;
            }

        }
    }
}
