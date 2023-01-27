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
    }

    public class recipientsDTO
    {
        public IList<signersDTO> signers { get; set; }
    }

    public class signersDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string recipientId { get; set; }
        public string roleName { get; set; }
    }


}
