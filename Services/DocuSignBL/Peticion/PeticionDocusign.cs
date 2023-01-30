using Model.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
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

            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwCAoEBF-gLbSAgAgOBjUz0D20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAACqhE-gLbSDcATFPDbC0aE02P4LYnQ8rjeA.dRA36MECJ9bEk_sIj_qYz8J_n-EggMr93SmhLb7BKHnrxxSTWyJrd6JvnTtVAtLwhHQjUT1LplmXgL6MiL9E-wgsH3DHqODfDW8Cr4iXe6vKIjej-DN7rYTnlLqE9hA1Sh-VGdIS8xzesuNTgR4uMNbZPpE4KKc1oW5nKcj_uroRIr92t4zf3z6LwdBmRkwWly5ENN67j7re8TdQf1X9eyWN-PGCzDG_nb_zi7UsjULk1AqGK7XjOPQ_O6HZeX_UYL21jUsR_jkFeOoEYP86xsK0fRhxAdtxyJKiuR2YH6BDW52hiie9h165azMXE7HnG-vO7PrkB2DbLKjiEqk2VA";

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
            auth.isAuthenticated = true;
            auth.URL = "https://account.docusign.com/oauth/auth?client_id=4907d311-b734-4d16-be6c-acab89356eaa&scope=signature&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%2FSinco%2FV3%2FADPRO%2FDocusign%2Fapi%2Fds%2Fcallback";
            return auth;
        }

    }
}
