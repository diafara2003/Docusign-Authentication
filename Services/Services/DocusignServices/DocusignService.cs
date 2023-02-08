using Model.DTO;
using Model.DTO.Docusign;
using Model.DTO.Users;
using Repository.Repositories.Peticion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Services.DocusignServices
{
    public interface IDocusignService
    {
        Task<T> peticion<T>(string method, MethodRequest type, object data = null);

        Task<T> peticionFile<T>(string method,  object data = null);

        AuthenticationDTO validationAuthentication();

        void AgregarToken(string token, int usuario, string RefreshToken);


        Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplates();

        Task<EnvelopeSignerDTO> GetRecipentsEnvelope(string envelopes);

        Task<Tuple<AuthenticationDTO, IList<envelopeTemplatesDTO>>> GetTemplatesSigners();


        Task<Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>> GetEnvelopeHistory(string idenvelope);
    }

    public class DocusignService : IDocusignService
    {

        private IPeticionDocusignRepository _peticionDOcusign;
        //public DocusignService(IPeticionDocusignRepository peticionDocusign)
        //{
        //    this._peticionDOcusign = peticionDocusign;
        //}

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
            _peticionDOcusign.AgregarToken(token, usuario, RefreshToken);
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
            return await _peticionDOcusign.peticionFile<T>(method,  data);
        }

        public AuthenticationDTO validationAuthentication()
        {
            return _peticionDOcusign.validationAuthentication();
        }
    }
}
