using Model.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata;
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

        
            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwAAaGggLwbbSAgAAKiLLnIG20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAAOzcfLwbbSDcATFPDbC0aE02P4LYnQ8rjeA.MxhTSN2JeECoGImTyXknyvfidved2HIVVl2vzWcjk4_gIBVrdY2-KIc4JlxJq7_NSXzPbt119Qp9MInGuRUxGoT00eJ74pnDUODkJM-tEj_QwXql1IQRJsGHEKRWCV9DpxCajz2KEXwhm4-LdgKqrPC49d72m0uF8JvYyBv58dHCzNA_6iam-JpIBmBTzjPCMrh7Z7TBq5Hwx0cHN5MSYiSlW9cjOBGLdj64ubziKr6s5FRxes4Obih-jATQwH8TSdy7hwZQNOBwVopuw7zpLMskExtnCcvUP-wg1fXg7r3vJeQ0ko9YPal2XfEwUG9C8yxX_oHEtIrJkyU7okAmHA";
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

        public async Task<T> peticionFile<T>(string method, HttpMethod type, object data = null) where T : class
        {

            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}"),
                Timeout = new TimeSpan(0, 2, 0)
            };

            var token = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiOGFlYzFjZjQtYmE4NS00MDM5LWE1MmItYzVhODAxMjA3N2EyIn0.AQsAAAABAAUABwCAyrze-AXbSAgAgArg7DsG20gCALv6cVlKS7RPn5vjUswLIFIVAAMAAAAYAAEAAAAFAAAADQAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhIgAkAAAANDkwN2QzMTEtYjczNC00ZDE2LWJlNmMtYWNhYjg5MzU2ZWFhEgABAAAACwAAAGludGVyYWN0aXZlMAAANCTe-AXbSDcATFPDbC0aE02P4LYnQ8rjeA.mZnWqN333cyOB9V9H2AcwNfo4WjSmhd2YeLbnE6RSXYx0TxhJ5kDhVPRW-5mZJKDfzp5u1MSMBiyxKhzdfXKJKGfvQzuS163lJxTCepd64d3XOpqTgCQf1mkYzYa1ptbPNfNgawBhuZgLg8F_VdiL51bDg7-HJkhOiKtbjIMr9o_aSAI7kP2cFGoGwd6ge14jh2v-u_6okcUm681DAyjhkTya1ivyhMYaLVouQ6xf-E43HDHLHG-WQPJLEZM_QMERkS1jACGQPL2stse_KKKY1Fxo00aiBpClHL5aF2Jco-qKxJsFkVok1JlosZr1RyiD2lxITHbEg4yZJyk1egowQ";

            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = new HttpResponseMessage();

            response = await httpClient.GetAsync($"https://na3.docusign.net/restapi/v2.1/accounts/56483961/{method}");

            byte[] content = await response.Content.ReadAsByteArrayAsync();

            string fileBase64 = Convert.ToBase64String(content);

            fileBase64 = "{\"documentBase64\" :\"" + fileBase64 + "\"}";

            var x = JsonConvert.DeserializeObject<T>(fileBase64);

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
