using DocuSignBL.Peticion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("token")]
        public IActionResult AccessAutorizacion(string code)
        {
            var usuario = new SincoSoft.Context.Core.CurrentContext(_httpContextAccessor);
            new PeticionDocusign(_httpContextAccessor).AgregarToken(code, usuario.IdUsuario, "");

            return Ok("");
        }
    }
}
