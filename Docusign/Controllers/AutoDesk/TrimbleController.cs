using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net;

namespace API_CORE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrimbleController : ControllerBase
    {

        const string CLIENT_ID = "170df479-0a65-4737-8153-9e9715c771ed";
        const string APP_NAME = "CSPluginTC";
        const string CALLBACK_URL = "http://localhost/sinco/v3/adpro/core/api/trimble/callback";
        const string CLIENT_SECRET = "G33fED4fXE1pErnvZpOK3MyGug4a";

        [HttpGet]
        public HttpResponseMessage Get()
        {
            string url =$"https://stage.id.trimblecloud.com/oauth/authorize?response_type=code&state=&client_id={CLIENT_ID}&scope=openid%20{APP_NAME}&redirect_uri={CALLBACK_URL}";

            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(url);
            return response;


        }

        [HttpGet("callback")]
        public IActionResult GetCallBack()
        {
            return Ok("calback endpoint");
        }
    }
}
