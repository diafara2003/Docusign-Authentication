using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public int ProCod { get; set; }
        public string ProDesc { get; set; }
        public string ProUnidadCont { get; set; }
        public string ProGrupo { get; set; }
        public decimal ProStockMinimo { get; set; }
        public string ProCodBIM { get; set; }
    }
}