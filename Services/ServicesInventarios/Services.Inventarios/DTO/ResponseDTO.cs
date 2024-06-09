using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventarios.DTO
{
    public class ResponseDTO
    {
        public int codigo { get; set; }
        public string? mensaje { get; set; } = null;
        public bool success { get; set; }

    }
}
