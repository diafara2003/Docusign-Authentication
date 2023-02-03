using Model.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DocuSignBL.Peticion
{
    public class PeticionDocusign
    {
        public async Task<T> peticion<T>(string method, HttpMethod type, object data = null) where T : class
        {

            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };


            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwAAIGmx6gXbSAgAAGCMvy0G20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAA8zew6gXbSDcATFPDbC0aE02P4LYnQ8rjeA.AT0qNz6ZOGQbupIWvjl4US99iY1r40VDmvjL43RpDgfYsKiabL9vrUp7wgGih31PatZoOqvVkrgGHD13vg8Mb9EnGeo6lboUrvh2xkWNTAM_xeh4NVHOnzZ2PSeY0Ur3-jBWP1FLeVwLPgnsc7rstzeKtx4EKvTihOQK_eCPqwFVWXQqqeeiS2eQ9B4lKP1w0OMQuMKWC42zxmmCkqrOyMXjBgjxZznL81rCnwqgRYLC5nhd8VRZZac-aG3bIkkvCH2oviB0CMzi0Vy6u_0MsyubV7wZKOv5q8wOfDPZyCUhV9Aii7XUowebpnlJRUbT6aBwsXUpYXaxJpcGAHPEqg";
            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = new HttpResponseMessage();

            if (type.Method == "GET")
            {
                response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}");
            }
            else if (type.Method == "POST")
            {
                var json = JsonConvert.SerializeObject(data);
                var dataEnvio = new StringContent(json, Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}", dataEnvio);
            }


            string content = string.Empty;

            using (StreamReader stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                content = stream.ReadToEnd();
            }

            var x = JsonConvert.DeserializeObject<T>(content);

            return x;
        }


        public AuthenticationDTO validationAuthentication()
        {
            AuthenticationDTO auth = new AuthenticationDTO();
            auth.isAuthenticated = true;
            auth.URL = "https://account.docusign.com/oauth/auth?client_id=4907d311-b734-4d16-be6c-acab89356eaa&scope=signature&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%2FSinco%2FV3%2FADPRO%2FDocusign%2Fapi%2Fds%2Fcallback";
            return auth;
        }

    }
}
