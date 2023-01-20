using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dsController : ControllerBase
    {
        [HttpGet("callback")]
        public IActionResult GetDocuSign(string code)
        {
            return Ok(code);
        }
    }
}
