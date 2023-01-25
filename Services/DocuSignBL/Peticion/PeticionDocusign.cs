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

            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwCAo-Sc1_7aSAgAgOMHqxr_2kgCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAADUyc1_7aSDcATFPDbC0aE02P4LYnQ8rjeA.AZW27pmX1_X1szr13pTRHzfH7LdtAvAhE4sXsMzAPioH4Iq1Gy3YW8B5EkJXjoWX94cR2vlREFyvqYR2kWr-EGlTZXXtIM7BwdfbWAMldVv-PooUQqKbOnTsEZjslNEKRKMoz6t1nWC3o4Hg6rR9PY818hUMRgqhlVLQhW6JQVvI1P5bf-GrcIiDG2d-nb2ezlXUOlqpANL7wjIF5TXd5TlmQ0FjHQQmCvROTaj2vY83alpYl3Z2CFswu0PhGCWQ7PX-qXKqMVnr2q0OwPO_I9gupyVL1_ceIhKrlDb807wKsHHQYiCbCzIj7pyX-4BvNkj0--8s2TMQEdzXrCuDmg";

            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}");
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
            auth.isAuthenticated = false;
            auth.URL = "https://account.docusign.com/oauth/auth?client_id=4907d311-b734-4d16-be6c-acab89356eaa&scope=signature&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%2FSinco%2FV3%2FADPRO%2FDocusign%2Fapi%2Fds%2Fcallback";
            return auth;
        }

    }
}
