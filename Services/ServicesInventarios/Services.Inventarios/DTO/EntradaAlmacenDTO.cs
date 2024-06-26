﻿using Model.DTO.ComprasD;
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


        public class TercerosDTO
        {
            public int id { get; set; }
            public string nombre { get; set; } = string.Empty;

        }

        public class CompraDTO
        {
            public int CompID { get; set; }
            public decimal CompTotalPagar { get; set; }
            public decimal CompTotalPagarMM { get; set; }
            public string MonAbrev { get; set; } = string.Empty;
            public string MonSimbolo { get; set; } = string.Empty;

        }
        public class DetallesCompraDTO
        {
            public DetallesCompraDTO()
            {
                this.entrada = new ADPEntradasAlmacen();
                this.compra = new ComprasDTO();
            }
            public ADPEntradasAlmacen entrada { get; set; }
            public ComprasDTO compra { get; set; }

        }
    }

    public class ListaEntradaAlmacenDTO
    {
        public int EnAID { get; set; }
        public int EnASuc { get; set; }
        public string Sucursal { get; set; } = string.Empty;
        public int EnAOC { get; set; }
        public int EnANo { get; set; }
        public string EnAReciboNo { get; set; } = string.Empty;
        public string TerNombre { get; set; } = string.Empty;
        public string EnAFecha { get; set; } = string.Empty;
        public string MvIVrTotalMM { get; set; } = string.Empty;
        public string EOrDesc { get; set; } = string.Empty;
        public string configuracion { get; set; } = string.Empty;
        public decimal CantEA { get; set; }
    }

    public class PendienteEntradaDTO
    {
        public int EnASuc { get; set; }
        public string NombreSuc { get; set; } = string.Empty;
        public int EnAOC { get; set; }
        public string TerNombre { get; set; } = string.Empty;
        public int TerID { get; set; }
        public int NoEntradas { get; set; }
        public string FechaUltimaEntrada { get; set; } = string.Empty;
    }

    public class CompraDTO
    {
        public int CompID { get; set; }
        public decimal CompTotalPagar { get; set; }
        public decimal CompTotalPagarMM { get; set; }
        public string MonSimbolo { get; set; }
        public string MonAbrev { get; set; }

    }

    public class TercerosDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }

    }
}
