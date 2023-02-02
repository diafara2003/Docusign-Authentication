using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System;
using DocuSignBL.Peticion;
using Model.DTO;
using Model.DTO.Users;
using System.Threading.Tasks;
using System.Collections.Generic;
using DocuSignBL.Opetations;
using System.Linq;
using System.Xml.Linq;

namespace Docusign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocusignController : ControllerBase
    {
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
                var x = await new PeticionDocusign().peticion<userDTO>("users", HttpMethod.Get);

                return Ok(x);

            }
            catch (Exception e)
            {

                return Ok(e.Message);
            }

        }

        [HttpGet("Templates")]
        public async Task<IActionResult> GetTemplates()
        {
            try
            {
                templatesDTO TemplatesArray = await new PeticionDocusign().peticion<templatesDTO>("templates?order_by=name", HttpMethod.Get);

                var TemplatesFilter = new List<envelopeTemplatesDTO>();

                foreach (var item in TemplatesArray.envelopeTemplates)
                {
                    if (item.name != "")
                    {
                        TemplatesFilter.Add(item);
                    }
                }

                var auth = new PeticionDocusign().validationAuthentication();
                Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>> responseAuth = new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, TemplatesFilter);

                return Ok(responseAuth);

            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }


        [HttpGet("TemplatesSigners")]
        public async Task<IActionResult> GetTemplatesSigners()
        {
            try
            {
                templatesDTO TemplatesArray = await new PeticionDocusign().peticion<templatesDTO>("templates?order_by=name&include=recipients,documents", HttpMethod.Get);
                var signers = TemplatesArray.envelopeTemplates;

                var auth = new PeticionDocusign().validationAuthentication();
                Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>> responseAuth = new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, signers);

                return Ok(responseAuth);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("envelopes/recipents")]
        public async Task<IActionResult> GetRecipentsEnvelope(string envelope)
        {
            return Ok(await new DocuSignBL.Opetations.DocuSignBL().GetRecipentsEnvelope(envelope));
        }

        [HttpPost("envelopes/send")]
        public async Task<IActionResult> SendEnvelope(EnvelopeSendDTO envelope)
        {
            try
            {
                templateDTO template = await new PeticionDocusign().peticion<templateDTO>("templates/" + envelope.IdTemplate, HttpMethod.Get);

                
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
                    string documentsBase64 = await new PeticionDocusign().peticion<string>(doc.uri, HttpMethod.Get);
                    doc.documentBase64 = documentsBase64;
                    documents.Add(doc);
                }

                documents.Add(docu);

                envelopeToSend.documents = documents;

                /*Se obtienen los firmantes*/

                /*Se obtienen contratista*/

                List<signersDTO> signers = new List<signersDTO>();

                signers.AddRange((from item in template.recipients.signers
                                  where item.email != "" && item.name != ""
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
                                                                                  }


                                      }

                                  }).ToList());


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


                envelopeResponse = await new PeticionDocusign().peticion<EnvelopeResponse>("envelopes", HttpMethod.Post, envelopeToSend);

                var auth = new PeticionDocusign().validationAuthentication();
                Tuple<AuthenticationDTO, EnvelopeResponse> responseAuth = new Tuple<AuthenticationDTO, EnvelopeResponse>(auth, envelopeResponse);

                return Ok(responseAuth);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

    }
}

