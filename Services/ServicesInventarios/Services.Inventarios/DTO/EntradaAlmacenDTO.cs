using Model.DTO.ComprasD;
using Model.Entity.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO.Inventarios
{
    public class EntradaAlmacenDTO
    {
        public ADPEntradasAlmacen entrada { get; set; }
        public ComprasDTO compra { get; set; }
    }

    public class DetalllesOCEADTO
    {
        public EntradaAlmacenDTO Encabezado { get; set; }
        public List<MovimientosInvDTO> Detalles { get; set; }
    }
}
