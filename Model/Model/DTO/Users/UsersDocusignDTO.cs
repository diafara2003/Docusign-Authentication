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
}
