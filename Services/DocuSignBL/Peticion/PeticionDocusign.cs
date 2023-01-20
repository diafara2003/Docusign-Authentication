using Model.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DocuSignBL.Peticion
{
    public class PeticionDocusign
    {
        public async Task<T> peticion<T>(string method, HttpMethod type, object data = null) where T : class
        {
            T result =  null;

            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwAABwqjCPvaSAgAAEctsUv72kgCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAA2tihCPvaSDcATFPDbC0aE02P4LYnQ8rjeA.f-s1AvBDVY9iaK0jdLxVTVQ4vnio1_fwiKeB_7X3pwEAjE8Dp1L7A-bWY581NVyjaqNGZlgmy4LjOsHF2QfZqhiHHeVrGhEjIhGM2jsEoz2ubmW6dqEf-y7q8f_Fnp4dAD83tk5cNHzq2k3x7J9hi6gCkSlbA_sxcK1rTV4aThXnQCzO_mjdw1wHP9xopJ6Ph8X2XAIDu8bUJ7Ekf414FXzSDI9L7jDf4APhrTDW0lpJ276qeRX81oGlv0GKQpS4fvPgPMdP1gGB5NozcjLrlnZj0Gp5k9-wib_jbIuewf-qa7awfT8rXTWcJxGKIJF63rZ3cTs1CHe8SdfLKFPaYg";

            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}");
            string content = string.Empty;

            using (StreamReader stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                content = stream.ReadToEnd();
            }

            var objDeserilize = JsonConvert.DeserializeObject<T>(content);

            return objDeserilize;
        }


        public AuthenticationDTO validationAuthentication()
        {
            AuthenticationDTO auth = new AuthenticationDTO();
            auth.isAuthenticated = false;
            auth.URL = "https://account.docusign.com/oauth/auth?client_id=4907d311-b734-4d16-be6c-acab89356eaa&scope=signature&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%2FSinco%2FV3%2FADPRO%2FDocusign%2Fapi%2Fds%2Fcallback";           
            return auth;
        }

    }
}
