using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.Docusign
{
    public class ResposeStateTokenDTO
    {
        public ResposeStateTokenDTO()
        {
            this.Cod = 0;
            this.Exist = false;
        }

        public int Cod { get; set; }
        public bool Exist{ get; set; }

    }
}
