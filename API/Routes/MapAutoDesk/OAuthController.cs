

using Microsoft.AspNetCore.Mvc;
using Autodesk.Forge.Client;
using System.Threading.Tasks;
using Autodesk.Forge;
using System;
using Repository.AutoDesk.forgeAPI;
using Microsoft.Extensions.Configuration;

namespace ForgeAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private static readonly Scope[] SCOPES = new Scope[] {
            Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch,
            Scope.BucketCreate, Scope.BucketRead, Scope.BucketUpdate, Scope.BucketDelete
        };
        private IConfiguration _configuration;

        public OAuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("token")]
        public async Task<IActionResult> auto()
        {
            string FORGE_CLIENT_ID = _configuration["FORGE_CLIENT_ID"];// Credentials.GetAppSetting("FORGE_CLIENT_ID");
            string FORGE_CLIENT_SECRET = _configuration["FORGE_CLIENT_SECRET"];//Credentials.GetAppSetting("FORGE_CLIENT_SECRET");
            try
            {
                string AccessToken = "";
                TwoLeggedApi _twoLeggedApi = new TwoLeggedApi();
                ApiResponse<dynamic> bearer = await _twoLeggedApi.AuthenticateAsyncWithHttpInfo(
                    FORGE_CLIENT_ID, FORGE_CLIENT_SECRET, oAuthConstants.CLIENT_CREDENTIALS, SCOPES);
                //httpErrorHandler(bearer, "Failed to get your token");
                AccessToken = bearer.Data.access_token;
                Credentials credentials = await Credentials.CreateFromCodeAsync(AccessToken, base.Response.Cookies);
                AccessToken response = new AccessToken();

                response.access_token = AccessToken;
                response.expires_in = 0;

                return Ok(response);
                //return (bearer);
            }
            catch (Exception ex)
            {

                return Ok(new AccessToken());
            }

        }

        public struct AccessToken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}
