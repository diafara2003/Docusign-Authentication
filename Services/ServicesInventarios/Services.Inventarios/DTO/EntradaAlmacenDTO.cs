using Model.DTO.ComprasD;
using Model.DTO.Docusign;
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
        public EntradaAlmacenDTO()
        {
            this.entrada = new ADPEntradasAlmacen();
            this.compra = new ComprasDTO();
        }
        public ADPEntradasAlmacen entrada { get; set; }
        public ComprasDTO compra { get; set; }
    }

    public class DetalllesOCEADTO
    {
        public DetalllesOCEADTO()
        {
            this.Encabezado = new EntradaAlmacenDTO();
            this.Detalles = new List<MovimientosInvDTO>();
        }
        public EntradaAlmacenDTO Encabezado { get; set; }
        public List<MovimientosInvDTO> Detalles { get; set; }
    }
}
