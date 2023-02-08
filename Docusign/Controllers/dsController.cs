
using Docusign.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dsController : ControllerBase
    {
        private IHttpContextAccessor httpContextAccessor;
        private IDocusignService docusignService;

        public dsController(IHttpContextAccessor _httpContextAccessor, IDocusignService _docusignService)
        {
            this.httpContextAccessor = _httpContextAccessor;
            this.docusignService = _docusignService;
        }

        [HttpGet("callback")]
        public IActionResult GetDocuSign(string code) => Ok(code);


        [HttpGet("callback/test")]
        public IActionResult GetTest()
        {
            var host = httpContextAccessor.HttpContext.Request.Host.Value;
            var path = httpContextAccessor.HttpContext.Request.PathBase.Value;

            string callback = $"https://{host}{path}/api/ds/callback";
            string callbackREplace = $"https://{host}{path}/api/ds/callback".Replace("/", "%2F").Replace(":", "%3A");

            return Ok(new
            {
                Url = callback,
                callbackReplace = callbackREplace,
            });
        }


        [HttpGet("callback/verificacion")]
        public IActionResult GetTokenVerificacion()
        {
           
            return Ok("");
        }

    }
}
