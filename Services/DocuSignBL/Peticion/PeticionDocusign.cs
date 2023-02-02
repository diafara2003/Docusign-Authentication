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

<<<<<<< HEAD
            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwAAO3wDXQTbSAgAAHufEaAE20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMACApOMCXQTbSDcATFPDbC0aE02P4LYnQ8rjeA.SgZM0bFz6YV1lDJz0XAZxpp6KMVkBD-BCdCu9dpKBnPTUoMQZ4xzyzTJH9ekzLBhuZzHeODFFal9ggOM0LUzCZoRZ85P5ce_PwTiggoHgDmO7PBxulBlwix5A3hdlg2H2mzKSz4z-Hd5TiuwlfN3pua7AQFQbIgdsx_H0i8S8A9wZi2_Via9p9JkI5wvv5H6sX0vBG1LiCMC7UAM7aVnvp7A-f1KBz953UGCiXRRCWAGU1hXycKha36S17tjyn_pcdsDMA1xYWjmxhrzMy2m-wwtfkmfNoq3euiQ6pxiae4JUH0NtquSjAEyY0HxfaRxUOx7mYbLwyeI0EnMfnOsVg";
=======
            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwCA9WxWJAXbSAgAgDWQZGcF20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAAX9RVJAXbSDcATFPDbC0aE02P4LYnQ8rjeA.tMtxBHJDhBwktpO47nhfm7bkyYpr6uSMiCwgmYQuFe-9v8jZc-PYNZuuxToJJqfb0p_9X0GD51cHlr1S9iFyQub_r20h7B3_4kl3otTwexo44X1MKAxEk_mZRdJdiABSJldy-Up-iy3TNMftFNp0OeZRUpsYTzOVU2HVEdDuPKm5RJLpVAJEpp9uHWLenZumey5Z8ydpqrsNN-O09psY3A7FBhSuPlcGSZuKrA9zl_i75SYRKkVnejPtkktVXn99wEYfvJ-THBnXE5g6Gi3M_i1xaEe_tHi4oO9c87eQAyi8OcErSd7gLIeoatpyxbHPWd5MlGeNn3eR9oyNxljTTA";

>>>>>>> 2a6dcfcc5294e409d3db236cd64f4bc409df1724
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
