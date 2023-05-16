
using System;
using System.Collections.Generic;

namespace Model.DTO.Docusign
{
    public class RecipientsDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public DateTime? signedDateTime { get; set; }
        public DateTime deliveredDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public string recipientType { get; set; }
    }


    public class EnvelopeSignerDTO
    {

        public List<EnvelopesDTO> envelopes { get; set; }
    }

    public class EnvelopesDTO
    {
        public string status{ get; set; }
        public string envelopeId { get; set; }
        public SignersDTO recipients { get; set; }

    }

    public class SignersDTO
    {
        public List<RecipientsDTO> signers { get; set; }
    }


    public class TemplateDocuSignDTO
    {
        public string name { get; set; }
        public SignersDTO recipients { get; set; }
    }
}

