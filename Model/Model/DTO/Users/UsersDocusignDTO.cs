using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.Users
{
    public class userDTO
    {
        public IList<UsersDocusignDTO> users { get; set; }
    }
    public class UsersDocusignDTO
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string isAdmin { get; set; }
        public string userId { get; set; }
    }
}
