using Model.DTO.Docusign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.Users
{
    public class userDTO
    {
        [JsonProperty]
        public IList<UsersDocusignDTO> users { get; set; }
    }
    public class UsersDocusignDTO
    {
        [JsonProperty]
        public string userName { get; set; }
        [JsonProperty]
        public string email { get; set; }
        [JsonProperty]
        public string isAdmin { get; set; }
        [JsonProperty]
        public string userId { get; set; }
    }

    public class templatesDTO
    {
        public IList<envelopeTemplatesDTO> envelopeTemplates { get; set; }
    }

    public class envelopeTemplatesDTO
    {
        public envelopeTemplatesDTO()
        {
            recipients = new recipientsDTO();
            recipients.carbonCopies = new List<carbonCopiesDtO>();
            recipients.signers = new List<signersDTO>();
        }
        public string templateId { get; set; }
        public string uri { get; set; }
        public string name { get; set; }
        public string emailSubject { get; set; }
        public string emailBlurb { get; set; }
        public string status { get; set; }
        public IList<documentsDTO> documents { get; set; }
        public recipientsDTO recipients { get; set; }

    }

    public class templateDTO
    {
        public string templateId { get; set; }
        public string uri { get; set; }
        public string name { get; set; }
        public string emailSubject { get; set; }
        public string emailBlurb { get; set; }
        public IList<documentsDTO> documents { get; set; }
        public recipientsDTO recipients { get; set; }

    }

    public class documentsDTO
    {
        public string documentId { get; set; }
        public string uri { get; set; }
        public string name { get; set; }
        public string documentBase64 { get; set; }
        public string fileExtension { get; set; }       
    }

    public class recipientsDTO
    {
        public IList<signersDTO> signers { get; set; }
        public IList<carbonCopiesDtO> carbonCopies { get; set; }
    }

    public class signersDTO
    {
        public signersDTO()
        {
            tabs= new tabsDTO();
            tabs.dateSignedTabs = new List<dateSignedTabsDTO>();
            tabs.fullNameTabs = new List<fullNameDTO>();
            tabs.signHereTabs = new List<signHereDTO>();
        }
        public string name { get; set; }
        public string email { get; set; }
        public string recipientId { get; set; }
        public string roleName { get; set; } 
        public string routingOrder { get; set; }
        public tabsDTO tabs { get; set; }

    }

    public class tabsDTO
    {
        public IList<dateSignedTabsDTO> dateSignedTabs { get; set; }
        public IList<fullNameDTO> fullNameTabs { get; set; }
        public IList<signHereDTO> signHereTabs { get; set; }
    }

    public class dateSignedTabsDTO
    {
        public string anchorString { get; set; }
        public string anchorYOffset { get; set; }
        public string fontSize { get; set; }
        public string name { get; set; }
        public string recipientId { get; set; }
        public string tabLabel { get; set; }
    }

    public class fullNameDTO
    {
        public string anchorString { get; set; }
        public string anchorYOffset { get; set; }
        public string fontSize { get; set; }
        public string name { get; set; }
        public string recipientId { get; set; }
        public string tabLabel { get; set; }
    }

    public class signHereDTO
    {
        public string anchorString { get; set; }
        public string anchorYOffset { get; set; }
        public string fontSize { get; set; }
        public string name { get; set; }
        public string optional { get; set; }
        public string recipientId { get; set; }
        public string scaleValue { get; set; }
        public string tabLabel { get; set; }
    }

    public class carbonCopiesDtO
    {
        public string email { get; set; }
        public string name { get; set; }
        public string recipientId { get; set; }
        public string routingOrder { get; set; }
    }

    public class EnvelopeSendDTO
    {
        public string documentoBase64 { get; set; }
        public string documentId { get; set; }
        public string fileExtension { get; set; }
        public string name { get; set; }
        public string IdTemplate { get; set; }
    }

    public class EnvelopeResponse
    {
        public string envelopeId { get; set; }

        public string uri { get; set; }

        public string statusDateTime { get; set; }

        public string status { get; set; }

    }
}
