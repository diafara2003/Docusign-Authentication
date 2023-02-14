using System;
using System.Collections.Generic;

namespace Model.DTO.Docusign
{
    public class DocusignAuditoriaDTO
    {
        public string emailSubject { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public List<EnvelopeDocumentsDTO> envelopeDocuments { get; set; }
        public recipientsAuditDTO recipients { get; set; }
        public string status { get; set; }
    }


    public class recipientsAuditDTO
    {
        public List<SignersAuditDTO> signers { get; set; }
    }

    public class SignersAuditDTO
    {
        public string name { get; set; }
        public string recipientType { get; set; }
        public string status { get; set; }
        public int completedCount { get; set; }
    }

    public class EnvelopeDocumentsDTO
    {
        public string name { get; set; }
        public string documentId { get; set; }
        public string uri { get; set; }
        public byte[] file { get; set; }

    }

    public class ResponseEnvelopeDTO
    {
        public ResponseEnvelopeDTO()
        {
            this.envelopeDocuments = new List<EnvelopeDocumentsDTO>();
        }
        public string envelopeId { get; set; }
        public List<EnvelopeDocumentsDTO> envelopeDocuments { get; set; }
    }

    public class EnvelopeDocusignAudit
    {
        public List<AuditEventsDocusignDTO> auditEvents { get; set; }
    }
    public class AuditEventsDocusignDTO
    {
        public List<EventFieldsDocuSignDTO> eventFields { get; set; }
    }
    public class EventFieldsDocuSignDTO
    {
        public EventFieldsDocuSignDTO()
        {
            this.value = string.Empty;
            this.name = string.Empty;
        }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ResponseDocusignAuditoriaDTO
    {
        public ResponseDocusignAuditoriaDTO()
        {
            this.encabezado = new ResponseEncabezadoAuditDTO();
            this.detalles = new List<ResponseDetalleAuditDTO>();
        }

        public ResponseEncabezadoAuditDTO encabezado { get; set; }
        public List<ResponseDetalleAuditDTO> detalles { get; set; }
    }


    public class ResponseDetalleAuditDTO
    {
        public string hora { get; set; }
        public string usuario { get; set; }
        public string accion { get; set; }
        public string actividad { get; set; }
        public string estado { get; set; }
    }
    public class ResponseEncabezadoAuditDTO
    {
        public string asunto { get; set; }
        public string documentos { get; set; }
        public string estado { get; set; }
        public string fechaEnvio { get; set; }
        public string fechaCreacion { get; set; }
        public string destinatarios { get; set; }

    }
}
