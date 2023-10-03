using Model.DTO.ComprasD;
using Model.Entity.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.Inventarios
{
    public class MovimientosInvDTO
    {
        public MovimientosInvDTO()
        {
            this.movimientosInv = new MovimientosInv();
            this.comprasDet = new ComprasDetDTO();
        }
        public MovimientosInv movimientosInv { get; set; }
        public ComprasDetDTO comprasDet { get; set; }
        public int DevAsociada { get; set; }
        public decimal CantidadPendiente { get; set; }
        public decimal CantMin { get; set; }
        public decimal CantMax { get; set; }
        public decimal CantInv { get; set; }
    }
}
