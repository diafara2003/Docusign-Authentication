using Microsoft.Extensions.Configuration;
using Model.DTO.Docusign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Docusign.Repository.Peticion
{

    public interface IPeticionDocusignAuth
    {
        Task<DocusignAuthDTO> GetAccesToken(string key,string code);
    }


    public class PeticionDocusignAuth : IPeticionDocusignAuth
    {
        private IConfiguration _configuration;
        public PeticionDocusignAuth(IConfiguration configuration)
        {
            this._configuration = configuration;

        }
        public async Task<DocusignAuthDTO> GetAccesToken(string key, string code)
        {
            HttpMessageHandler handler = new HttpClientHandler();
            string clientId = _configuration[$"DocuSign_{key}:ClientId"];
            string ClientSecret = _configuration[$"DocuSign_{key}:ClientSecret"];


            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://account.docusign.com/oauth/token"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{ClientSecret}");

            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + System.Convert.ToBase64String(plainTextBytes));

            HttpResponseMessage response = new HttpResponseMessage();




            var data = new[]
{
    new KeyValuePair<string, string>("grant_type", "authorization_code"),
    new KeyValuePair<string, string>("code", code),
};
            try
            {
                response = await httpClient.PostAsync($"https://account.docusign.com/oauth/token", new FormUrlEncodedContent(data));
            }
            catch (Exception e)
            {

                throw e;
            }




            string content = string.Empty;

            using (StreamReader stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                content = stream.ReadToEnd();
            }

            var x = JsonConvert.DeserializeObject<DocusignAuthDTO>(content);

            return x;
        }
    }
}
