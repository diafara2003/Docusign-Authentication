using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using Model.DTO;
using Model.DTO.Users;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Model.DTO.Docusign;
using Docusign.Services;
using Docusign.Middleware;
using Docusign.Repository.Peticion;
using Microsoft.AspNetCore.Hosting;
using System.IO.Compression;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;
using DocumentFormat.OpenXml.Vml.Office;

namespace Docusign.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocusignController : ControllerBase
    {
        private readonly IDocusignService _docusignService;
        private readonly IEjemplo _ejemplo;
        IWebHostEnvironment _webHostEnvironment;

        public DocusignController
           (IDocusignService peticionDocusign, IWebHostEnvironment webHostEnvironment)
        //(IEjemplo ejemplo)
        {
            this._docusignService = peticionDocusign;
            this._webHostEnvironment = webHostEnvironment;
            //  this._ejemplo = ejemplo;

        }


        [HttpGet("docusign")]
        public IActionResult GetDocuSign()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/Docusign/userinfo" });
        }

        [HttpGet("userInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            //if (!User.Identity.IsAuthenticated) return Challenge(new AuthenticationProperties() { RedirectUri = "/Docusign/userInfo" });

            //var auth = new PeticionDocusign().validationAuthentication();
            //if (!auth.isAuthenticated)
            //{
            //    Tuple<AuthenticationDTO, stringDocusign/SignersByTemplete responseAuth = new Tuple<AuthenticationDTO, string>(auth, string.Empty);
            //    return Ok(responseAuth);
            //}
            try
            {
                var x = await _docusignService.peticion<userDTO>("users", MethodRequest.GET);

                return Ok(x);

            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }

        }

        /// <summary>
        /// obtiene todos los templates de docusign
        /// </summary>
        /// <returns></returns>
        [HttpGet("Templates")]
        public async Task<IActionResult> GetTemplates()
        {
            try
            {
                return Ok(await _docusignService.GetTemplates());
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        /// <summary>
        /// obtiene todos los firmantes de un  templates de la cuenta DOcuSign
        /// </summary>
        /// <returns></returns>
        [HttpGet("TemplatesSigners")]
        public async Task<IActionResult> GetTemplatesSigners()
        {
            try
            {
                return Ok(await _docusignService.GetTemplatesSigners());
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        /// <summary>
        /// obtiene todos los firmantes de un sobre
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        [HttpGet("envelopes/recipents")]
        public async Task<IActionResult> GetRecipentsEnvelope(string envelope)
        {
            return Ok(await _docusignService.GetRecipentsEnvelope(envelope));
        }

        /// <summary>
        /// Metodo encargado de consultar los firmantes segun un template especifico
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        [HttpGet("SignersByTemplete")]
        public async Task<IActionResult> GetSignersByTemplete(string idTemplate, string contrato = "")
        {
            try
            {

                return Ok(await _docusignService.GetSignersByTemplete(idTemplate, contrato));
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("envelopes/send")]
        public async Task<IActionResult> SendEnvelope(EnvelopeSendDTO envelope)
        {
            try
            {
                return Ok(await _docusignService.SendEnvelope(envelope));
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("envelopes/status")]
        public async Task<IActionResult> GetEnvelopeStatusSigners(string idenvelope)
        {
            var response = await _docusignService.peticion<DocusignAuditoriaDTO>($"envelopes/{idenvelope}?include=recipients,documents", MethodRequest.GET);


            return Ok(new Tuple<AuthenticationDTO, DocusignAuditoriaDTO>(new AuthenticationDTO()
            {
                isAuthenticated = true,
            }, response));
        }

        /// <summary>
        /// Metodo encargado de consultar los firmantes segun un template especifico
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        [HttpGet("envelopes/history")]
        public async Task<IActionResult> GetEnvelopeHistory(string idenvelope)
        {
            try
            {
                return Ok(await _docusignService.GetEnvelopeHistory(idenvelope));
            }
            catch (Exception e)
            {
                return Ok(new Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>(new AuthenticationDTO() { isAuthenticated = true }, new ResponseDocusignAuditoriaDTO()));
            }
        }

        /// <summary>
        /// Metodo encargado de consultar los firmantes segun un template especifico
        /// </summary>
        /// <param name="idTemplate"></param>
        /// <returns></returns>
        [HttpGet("envelopes/documents")]
        public async Task<IActionResult> GetEnvelopeDocuments(string idenvelope)
        {
            try
            {
                return Ok(await _docusignService.GetEnvelopeDocuments(idenvelope));
            }
            catch (Exception e)
            {
                return Ok(new Tuple<AuthenticationDTO, ResponseEnvelopeDTO>(new AuthenticationDTO() { isAuthenticated = true }, new ResponseEnvelopeDTO()));
            }
        }

        /// <summary>
        /// Metodo para devolver zip
        /// </summary>
        /// <param name="idenvelope"></param>
        /// <returns></returns>
        [HttpGet("envelopes/documents/zip")]
        public async Task<FileResult> GetDocumentsZip(string idenvelope)
        {
            Tuple<AuthenticationDTO, ResponseEnvelopeDTO> respuesta = await _docusignService.GetEnvelopeDocuments(idenvelope);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, false))
                {
                    foreach (var item in respuesta.Item2.envelopeDocuments)
                    {
                        ZipArchiveEntry fileInArchive = zipArchive.CreateEntry(item.name + ".pdf", CompressionLevel.Optimal);

                        using (var entryStream = fileInArchive.Open())
                        {
                            using (var ms = new MemoryStream(item.file))
                            {
                                await ms.CopyToAsync(entryStream);
                            }
                        }
                    }
                }

                return File(memoryStream.ToArray(), "application/x-zip-compressed", "test-txt-files.zip");
            }
        }

        [HttpGet("state/token")]
        public IActionResult GetStateToken()
        {
            try
            {
                return Ok(_docusignService.StateToken(_webHostEnvironment.ContentRootPath));
            }
            catch (Exception e)
            {
                return Ok(new ResposeStateTokenDTO());
            }
        }

        [HttpGet("validarsesion")]
        public IActionResult GetValdiadSesion()
        {

            return Ok(new Tuple<AuthenticationDTO, string>(_docusignService.validationAuthentication(), string.Empty));
        }

    }
}

