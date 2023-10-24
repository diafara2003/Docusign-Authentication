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
    public class EntradaAlmacenTableDTO
    {
        public EntradaAlmacenTableDTO()
        {
            this.entrada = new ADPEntradasAlmacen();
            this.compra = new ComprasDTO();
            this.Respuesta = new ResponseV3DTO();
        }
        public ADPEntradasAlmacen entrada { get; set; }
        public ComprasDTO compra { get; set; }
        public ResponseV3DTO Respuesta { get; set; }
    }

    public class DetalllesOCEADTO
    {
        public DetalllesOCEADTO()
        {
            this.Encabezado = new EntradaAlmacenTableDTO();
            this.Detalles = new List<MovimientosInvDTO>();
        }
        public EntradaAlmacenTableDTO Encabezado { get; set; }
        public List<MovimientosInvDTO> Detalles { get; set; }
    }

    public class EntradaAlmacenDTO
    {
        public EntradaAlmacenDTO()
        {
            this.movimientosInv = new MovimientosInv();
        }
        public MovimientosInv movimientosInv { get; set; }

        public int EnAID { get; set; }
        public int EnANo { get; set; }
        public string EnAFecha { get; set; } = string.Empty;
        public int EnASuc { get; set; }
        public int EnAObra { get; set; }
        public int EnAOC { get; set; }
        public int EnAUsu { get; set; }
        public int Bodega { get; set; }
        public int SoloEncabezado { get; set; }
        public string FechaMin { get; set; } = string.Empty;
        public string EnAObs { get; set; } = string.Empty;
        public string EnAFac { get; set; } = string.Empty;
        public string EnAFechaFac { get; set; } = string.Empty;     
        public string EnAFechaPago { get; set; } = string.Empty;
        public string EnAReciboNo { get; set; } = string.Empty;
        public string EnAFechaReciboNo { get; set; } = string.Empty;
        public string EnASticker { get; set; } = string.Empty;
    }
}
