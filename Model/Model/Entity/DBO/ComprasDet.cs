using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("ComprasDet")]
    public class ComprasDet
    {
        [Key]
        public int CompDetID { get; set; }
        public int CompDetCompras { get; set; }
        public int CompDetProd { get; set; }
        public decimal CompDetCant { get; set; }
        public decimal CompDetIVA { get; set; }
        public DateTime CompDetFechaReq { get; set; }
        public decimal CompDetUnitarioMM { get; set; }
        public decimal CompDetBaseIvaDiff { get; set; }
        public decimal CompDetBaseIvaDiff2 { get; set; }
    }
}
