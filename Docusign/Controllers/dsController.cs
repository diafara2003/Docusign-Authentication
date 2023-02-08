
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Repositories.Peticion;

namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dsController : ControllerBase
    {

        private IHttpContextAccessor _httpContextAccessor { get; }

        public dsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("callback")]
        public IActionResult GetDocuSign(string code)
        {
           
            return Ok(code);
        }

       
    }
}
