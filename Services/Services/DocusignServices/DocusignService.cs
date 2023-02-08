﻿using Model.DTO;
using Model.DTO.Docusign;
using Model.DTO.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Docusign.Repository.Peticion;
using Docusign.Repository.DataBase.Conexion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Docusign.Services
{
    public interface IDocusignService
    {
        Task<T> peticion<T>(string method, MethodRequest type, object data = null);

        Task<T> peticionFile<T>(string method, object data = null);

        AuthenticationDTO validationAuthentication();

        void AgregarToken(string token, int usuario, string RefreshToken);


        Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplates();

        Task<EnvelopeSignerDTO> GetRecipentsEnvelope(string envelopes);

        Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplatesSigners();

        Task<Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>> GetEnvelopeHistory(string idenvelope);
        Task<Tuple<AuthenticationDTO, EnvelopeResponse>> SendEnvelope(EnvelopeSendDTO envelope);
        ResposeStateTokenDTO StateToken(string rootWeb);
    }

    public class DocusignService : IDocusignService
    {

        private IPeticionDocusignRepository _peticionDOcusign;
        private IDocusignCallbackService _peticionDocuCallBackService;
        private DB_ADPRO _contexto;
        public DocusignService(IPeticionDocusignRepository peticionDocusign, IDocusignCallbackService docuCallBackService, DB_ADPRO contexto)
        {
            this._peticionDOcusign = peticionDocusign;
            this._peticionDocuCallBackService = docuCallBackService;
            this._contexto = contexto;
        }

        public async Task<Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>> GetEnvelopeHistory(string idenvelope)
        {
            var auth = validationAuthentication();
            if (!auth.isAuthenticated)
            {
                return new Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>(auth, new ResponseDocusignAuditoriaDTO());
            }

            DocusignAuditoriaDTO SignersDocuemnts = await peticion<DocusignAuditoriaDTO>($"envelopes/{idenvelope}?include=documents,recipients", MethodRequest.GET);


            EnvelopeDocusignAudit EnvelopeAudit = await peticion<EnvelopeDocusignAudit>($"envelopes/{idenvelope}/audit_events", MethodRequest.GET);


            Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO> responseAuth = new Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>(auth, new ResponseDocusignAuditoriaDTO()
            {
                encabezado = new ResponseEncabezadoAuditDTO()
                {
                    documentos = SignersDocuemnts.envelopeDocuments != null
                        ? string.Join(", ", SignersDocuemnts.envelopeDocuments.Select(c => c.name))
                        : "",
                    asunto = SignersDocuemnts.emailSubject,
                    destinatarios = SignersDocuemnts.recipients != null && SignersDocuemnts.recipients.signers != null
                        ? string.Join(", ", SignersDocuemnts.recipients.signers.Where(c => c.recipientType == "signer").Select(c => c.name))
                        : "",
                    fechaCreacion = SignersDocuemnts.createdDateTime.ToString("MM/dd/yyyy h:mm tt"),
                    fechaEnvio = SignersDocuemnts.sentDateTime.ToString("MM/dd/yyyy h:mm tt"),
                    estado = StatusEnvelope(SignersDocuemnts.status)
                },
                detalles = GenerateDetail(EnvelopeAudit)

            });

            return responseAuth;
        }

        string StatusEnvelope(string name)
        {
            string statrusName = string.Empty;

            if (name == "sent") statrusName = "Enviado";
            else statrusName = "Completado";

            return statrusName;
        }

        List<ResponseDetalleAuditDTO> GenerateDetail(EnvelopeDocusignAudit data)
        {
            List<ResponseDetalleAuditDTO> objLst = new List<ResponseDetalleAuditDTO>();


            if (data.auditEvents == null) return objLst;


            data.auditEvents.ForEach(c =>
            {
                var accion = c.eventFields.FirstOrDefault(e => e.name.ToLower().Equals("action")) ?? new EventFieldsDocuSignDTO() { };
                var user = c.eventFields.FirstOrDefault(e => e.name.ToLower().Equals("username")) ?? new EventFieldsDocuSignDTO() { };
                var status = c.eventFields.FirstOrDefault(e => e.name.ToLower().Equals("envelopestatus")) ?? new EventFieldsDocuSignDTO() { };
                var date = c.eventFields.FirstOrDefault(e => e.name.ToLower().Equals("logtime")) ?? new EventFieldsDocuSignDTO() { };
                var message = c.eventFields.FirstOrDefault(e => e.name.ToLower().Equals("message")) ?? new EventFieldsDocuSignDTO() { };


                objLst.Add(new ResponseDetalleAuditDTO()
                {
                    accion = accion != null ? accion.value : string.Empty,
                    actividad = message != null ? message.value : string.Empty,
                    estado = status != null ? status.value : string.Empty,
                    hora = status != null ? Convert.ToDateTime(date.value).ToString("MM/dd/yyyy h:mm tt") : string.Empty,
                    usuario = user != null ? user.value : string.Empty,
                });

            });

            return objLst;
        }

        public async Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplatesSigners()
        {
            var auth = validationAuthentication();

            if (!auth.isAuthenticated)
            {
                new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, new List<envelopeTemplatesDTO>());
            }

            templatesDTO TemplatesArray = await peticion<templatesDTO>("templates?order_by=name&include=recipients,documents", MethodRequest.GET);
            var signers = TemplatesArray.envelopeTemplates;


            return new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, signers);

        }

        public void AgregarToken(string token, int usuario, string RefreshToken)
        {

            _contexto.tokenDocusign.ToList().ForEach(c => _contexto.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);

            _contexto.tokenDocusign.Add(new Model.Entity.ADP_API.TokenDocusign()
            {
                TokenDocuId = 0,
                EnProceso = false,
                Token = token,
                Fecha = DateTime.Now,
                IdUsuario = usuario,
                RefreshToken = RefreshToken

            });

            _contexto.SaveChanges();
            //_peticionDOcusign.AgregarToken(token, usuario, RefreshToken);
            //_peticionDocuCallBackService.ReadTokenFile(token);
        }

        public async Task<EnvelopeSignerDTO> GetRecipentsEnvelope(string envelopes)
        {
            return await peticion<EnvelopeSignerDTO>($"envelopes?envelope_ids={envelopes}&include=recipients", MethodRequest.GET);
        }

        public async Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplates()
        {
            var auth = validationAuthentication();

            if (!auth.isAuthenticated)
            {
                return new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, new List<envelopeTemplatesDTO>());
            }


            templatesDTO TemplatesArray = await _peticionDOcusign.peticion<templatesDTO>("templates?order_by=name", MethodRequest.GET);

            var TemplatesFilter = new List<envelopeTemplatesDTO>();

            if (TemplatesArray.envelopeTemplates != null)
                foreach (var item in TemplatesArray.envelopeTemplates)
                {
                    if (item.name != "")
                    {
                        TemplatesFilter.Add(item);
                    }
                }


            return new Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>(auth, TemplatesFilter);

        }

        public async Task<T> peticion<T>(string method, MethodRequest type, object data = null)
        {
            return await _peticionDOcusign.peticion<T>(method, type, data);
        }

        public async Task<T> peticionFile<T>(string method, object data = null)
        {
            return await _peticionDOcusign.peticionFile<T>(method, data);
        }

        public AuthenticationDTO validationAuthentication()
        {
            return _peticionDOcusign.validationAuthentication();
        }
        public async Task<Tuple<AuthenticationDTO, EnvelopeResponse>> SendEnvelope(EnvelopeSendDTO envelope)
        {
            templateDTO template = await peticion<templateDTO>("templates/" + envelope.IdTemplate, MethodRequest.GET);


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
                    documentsDTO fileBase64 = await peticionFile<documentsDTO>("templates/" + envelope.IdTemplate + "/documents/" + doc.documentId, MethodRequest.GET);
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


            envelopeResponse = await peticion<EnvelopeResponse>("envelopes", MethodRequest.POST, envelopeToSend);

            return new Tuple<AuthenticationDTO, EnvelopeResponse>(new AuthenticationDTO()
            {
                isAuthenticated = true,

            }, envelopeResponse);
        }

        public ResposeStateTokenDTO StateToken(string rootWeb)
        {
            ResposeStateTokenDTO response = new ResposeStateTokenDTO();
            //Valida si existe un token guardado en tabla
            if (_contexto.tokenDocusign.Count() > 0)
                return response;
            //Valida si existe un archivo guardado
            else
            {
                var _token = _peticionDocuCallBackService.ReadTokenFile(rootWeb);
                if (_token == string.Empty)                
                    return response;                
                else
                {
                    AgregarToken(_token, 1, "");
                    _peticionDocuCallBackService.DeleteTokenFile(rootWeb);
             
                    response.Exist = true;
                    response.Cod = 1;
                    return response;
                }
            }           
        }        
    }
}
