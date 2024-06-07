using Autodesk.Forge.Client;
using Autodesk.Forge;
using Repository.AutoDesk.forgeAPI;

namespace API.Routes.MapAutodesk
{
    public static class MapOAuthRoutes
    {
        private static readonly Scope[] SCOPES = new Scope[] {
            Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch,
            Scope.BucketCreate, Scope.BucketRead, Scope.BucketUpdate, Scope.BucketDelete
        };
        private static Credentials Credentials { get; set; }
        public static void RegisterOAuth(this IEndpointRouteBuilder app)
        {

            app.MapGet("/EDT/BIM360/OAuth/token", async (HttpContext _httpContext,IConfiguration _configuration) =>
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
                    Credentials = await Credentials.CreateFromCodeAsync(AccessToken, _httpContext.Response.Cookies);
                    AccessToken response = new AccessToken();

                    response.access_token = AccessToken;
                    response.expires_in = 0;

                    return Results.Ok(response);
                    //return (bearer);
                }
                catch (Exception ex)
                {

                    return Results.Ok(new AccessToken());
                }
            }).WithTags("BIM360");
        }
        public struct AccessToken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}
