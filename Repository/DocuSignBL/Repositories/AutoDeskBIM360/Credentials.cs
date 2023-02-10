using Autodesk.Forge;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AutoDesk.forgeAPI
{
    public class Credentials
    {
        private const string FORGE_COOKIE = "ForgeApp";

        private Credentials() { }

        public string TokenInternal { get; set; }
        public string TokenPublic { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

        public static async Task<Credentials> FromSessionAsync(IRequestCookieCollection requestCookie, IResponseCookies responseCookie)
        {
            if (requestCookie == null || !requestCookie.ContainsKey(FORGE_COOKIE)) return null;

            Credentials credentials = JsonConvert.DeserializeObject<Credentials>(requestCookie[FORGE_COOKIE]);
            //if (credentials.ExpiresAt < DateTime.Now)
            //{
            //    await credentials.RefreshAsync();
            //    responseCookie.Delete(FORGE_COOKIE);
            //    responseCookie.Append(FORGE_COOKIE, JsonConvert.SerializeObject(credentials));
            //}

            return credentials;
        }


        public static async Task<Credentials> CreateFromCodeAsync(string code, IResponseCookies cookies)
        {
            ThreeLeggedApi oauth = new ThreeLeggedApi();



            Credentials credentials = new Credentials();
            credentials.TokenInternal = code;
            credentials.TokenPublic = code;

            //credentials.ExpiresAt = DateTime.Now.AddSeconds(credentialInternal.expires_in);

            cookies.Append(FORGE_COOKIE, JsonConvert.SerializeObject(credentials));

            return credentials;
        }
    }
}
