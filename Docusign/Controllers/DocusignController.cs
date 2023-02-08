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

namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocusignController : ControllerBase
    {
        private readonly IDocusignService _docusignService;
        private readonly IEjemplo _ejemplo;
        public DocusignController
           (IDocusignService peticionDocusign)
        //(IEjemplo ejemplo)
        {
            this._docusignService = peticionDocusign;
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
            //    Tuple<AuthenticationDTO, string> responseAuth = new Tuple<AuthenticationDTO, string>(auth, string.Empty);
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
        public async Task<IActionResult> GetSignersByTemplete(string idTemplate)
        {
            try
            {

                return Ok(await _docusignService.peticion<envelopeTemplatesDTO>($"templates/{idTemplate}/signers?order_by=name", MethodRequest.GET));
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
                templateDTO template = await _docusignService.peticion<templateDTO>("templates/" + envelope.IdTemplate, MethodRequest.GET);


                EnvelopeResponse envelopeResponse = new EnvelopeResponse();
                envelopeTemplatesDTO envelopeToSend = new envelopeTemplatesDTO();




                envelopeToSend.emailSubject = template.emailSubject;

                /*Se obtienen los documentos*/

                documentsDTO docu = new documentsDTO();
                List<documentsDTO> documents = new List<documentsDTO>();

                docu.documentBase64 = envelope.documentoBase64;
                docu.documentId = envelope.documentId;
                docu.fileExtension = envelope.fileExtension;
                docu.name = envelope.name;

                foreach (var doc in template.documents)
                {
                    if (doc.documentId != envelope.documentId)
                    {
                        documentsDTO fileBase64 = await _docusignService.peticionFile<documentsDTO>("templates/" + envelope.IdTemplate + "/documents/" + doc.documentId, HttpMethod.Get);
                        doc.documentBase64 = fileBase64.documentBase64;
                        doc.fileExtension = "pdf";
                        documents.Add(doc);
                    }
                }

                documents.Add(docu);

                envelopeToSend.documents = documents;

                //EnvelopeResponse envelopeResponse2 = new EnvelopeResponse();

                //envelopeResponse2.envelopeId = "467489b2-fddc-4264-977c-fe4944806c71";
                //envelopeResponse2.uri = "/envelopes/467489b2-fddc-4264-977c-fe4944806c71";
                //envelopeResponse2.statusDateTime = "2023-05-05T16:33:16.2970000Z";
                //envelopeResponse2.status = "sent";



                //var auth2 = new PeticionDocusign().validationAuthentication();
                //Tuple<AuthenticationDTO, EnvelopeResponse> responseAuth2 = new Tuple<AuthenticationDTO, EnvelopeResponse>(auth2, envelopeResponse2);
                //return Ok(responseAuth2);

                /*Se obtienen los firmantes*/

                /*Se obtienen firmantes docising*/

                List<signersDTO> signers = new List<signersDTO>();

                signers.AddRange((from item in template.recipients.signers
                                  where item.email != "" && item.name != "" && item.roleName != "" && !item.roleName.ToLower().Contains("contratista")
                                  select new signersDTO
                                  {
                                      email = item.email,
                                      name = item.name,
                                      recipientId = item.recipientId,
                                      routingOrder = item.routingOrder,
                                      tabs = new tabsDTO
                                      {
                                          signHereTabs = new List<signHereDTO>() {new signHereDTO(){
                                                                                       anchorString = string.Concat("/" + item.roleName.Replace(' ', '_')),
                                                                                       anchorYOffset = "-6",
                                                                                       name = item.name,
                                                                                       optional = "false",
                                                                                       recipientId = item.recipientId,
                                                                                       scaleValue = "1"
                                                                                    }
                                          },
                                      }

                                  }).ToList());

                /*Se obtienen contratista*/

                signersDTO contratista = new signersDTO();

                contratista = template.recipients.signers.Where(c => c.roleName.ToLower().Contains("contratista")).FirstOrDefault();

                contratista.email = envelope.emailTer;
                contratista.name = envelope.nameTer;
                contratista.tabs.signHereTabs = new List<signHereDTO>(){new signHereDTO(){
                                                                                       anchorString = string.Concat("/contratista"),
                                                                                       anchorYOffset = "-6",
                                                                                       name = envelope.nameTer,
                                                                                       optional = "false",
                                                                                       recipientId = contratista.recipientId,
                                                                                       scaleValue = "1"
                                                                                    }
                };

                signers.Add(contratista);
                envelopeToSend.recipients.signers = signers;

                /*Se obtienen los usuarios para copias*/

                foreach (var carbonCopies in template.recipients.carbonCopies)
                {
                    if (carbonCopies.email != "" && carbonCopies.name != "")
                    {
                        envelopeToSend.recipients.carbonCopies.Add(carbonCopies);
                    }
                }

                envelopeToSend.status = "sent";


                envelopeResponse = await _docusignService.peticion<EnvelopeResponse>("envelopes", MethodRequest.POST, envelopeToSend);

                var auth = _docusignService.validationAuthentication();
                Tuple<AuthenticationDTO, EnvelopeResponse> responseAuth = new Tuple<AuthenticationDTO, EnvelopeResponse>(auth, envelopeResponse);

                return Ok(responseAuth);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
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

    }
}

