
using Docusign.Services;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Model.DTO;

namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dsController : ControllerBase
    {

        private IDocusignCallbackService docusignService;
        IWebHostEnvironment webHostEnvironment;

        public dsController(IDocusignCallbackService _docusignService, IWebHostEnvironment _webHostEnvironment)
        {

            this.docusignService = _docusignService;
            this.webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet("callback")]
        public IActionResult GetDocuSign(string code)
        {
            docusignService.SaveTokenFile(code, webHostEnvironment.ContentRootPath);

            return Ok("Se autentico en DocuSign correctamente, puede cerrar esta ventana.");
        }


        [HttpGet("callback/verificacion")]
        public IActionResult GetTokenVerificacion()
        {

            return Ok(docusignService.ReadTokenFile(webHostEnvironment.ContentRootPath));
        }

    }
}
