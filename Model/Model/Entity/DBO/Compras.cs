using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("Compras")]
    public class Compras
    {
        [Key]
        public int CompID { get; set; }
        public int CompNo { get; set; }
        public int CompProv { get; set; }
        public int CompSuc { get; set; }
        public Int16 CompEstado { get; set; }
        public int CompFormaPago { get; set; }
        public DateTime CompFecha { get; set; }
        public decimal CompTotalPagar { get; set; }
        public decimal CompTotalPagarMM { get; set; }
        public Int16 CompEstadoReal { get; set; }
        public Int16 CompMoneda { get; set; }
        public string CompSitioEnt { get; set; }
        public string CompDesc { get; set; }
        public DateTime CompFechaReq { get; set; }
        public DateTime CompFechaReal { get; set; }
        public string CompObs { get; set; }
        public decimal CompMonedaTC { get; set; }
    }
}