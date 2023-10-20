using Autodesk.Forge.Model;
using Docusign.Repository.DataBase.Conexion;
using Inventarios.DTO;
using Microsoft.AspNetCore.Http;
using Model.DTO.ComprasD;
using Model.DTO.Inventarios;
using Model.Entity.DBO;
using Repository.DataBase.Conexion;
using System.Data;
using System.Text.RegularExpressions;

namespace Services.Inventarios
{
    public interface IEntradasService
    {
        List<ADPConfig> ConfigEntradas();
        List<BodegasSucursalDTO> ConsultaBodegas(string suc, string usuario);
        List<Terceros> TercerosEntradas(string filter, string suc);
        List<ComprasDTO> ComprasProveedor(string proveedor, string suc);
        DetalllesOCEADTO ConsultaDetallesOC(string compra, string suc);
    }
    public class EntradasServices : IEntradasService
    {
        private IHttpContextAccessor _httpContextAccessor { get; }
        private DB_ADPRO _contexto;

        public EntradasServices(DB_ADPRO contexto, IHttpContextAccessor httpContextAccessor)
        {
            this._contexto = contexto;
            this._httpContextAccessor = httpContextAccessor;
        }

        public List<ADPConfig> ConfigEntradas()
        {
            List<ADPConfig> configEntradas = new List<ADPConfig>
            {
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EaSticker").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EaBloqAmortizacion").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EditFacturaEA").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EAMvIReciboReq").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "Region").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "AlmMultiMoneda").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EA_RemisionAlfNum").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EA_ValidaRemNo").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "CompPedFechaRq").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "MostVrUnit_EA").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "BaseIva").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EditVrEASup").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "ADP_EAModVrPorc").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "PermiteEdicionEA_Vr").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "EaBloqAmortizacion").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" },
                _contexto.adpconfig.Where(c => c.CnfCodigo == "BloqFrameAmortizac").FirstOrDefault() ?? new ADPConfig() { CnfValor = "0" }
            };

            return configEntradas;
        }

        public List<BodegasSucursalDTO> ConsultaBodegas(string suc, string usuario)
        {
            List<BodegasSucursalDTO> dataRespuesta = new List<BodegasSucursalDTO>();
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            try
            {
                parametros.Add("suc", suc);
                parametros.Add("usuario", usuario);

                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor, new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_API_INV].[BodegasSucursalXUsuario]",
                    parametros = parametros
                });

                dataRespuesta = (from data in resultado.AsEnumerable()
                                 select new BodegasSucursalDTO()
                                 {
                                     Bodega = new BodegasSucursal()
                                     {
                                         BoSAutoNum = (int)data["BoSAutoNum"],
                                         BoSDescripcion = (string)data["BoSDescripcion"],
                                         BoSId = (int)data["BoSId"],
                                         BoSucursal = (short)data["BoSucursal"]
                                     }, 
                                     FechaMaxMov = (string)data["FechaMaxMov"]
                                 }).ToList();

            }
            catch (Exception e)
            {
                dataRespuesta = new List<BodegasSucursalDTO>();
            }

            return dataRespuesta;
        }

        public List<Terceros> TercerosEntradas(string filter, string suc)
        {
            List<Terceros> terceros = new List<Terceros>();

            if (!string.IsNullOrEmpty(suc))
            {
                terceros = (from T in _contexto.tercero
                            join C in _contexto.compras on T.TerID equals C.CompProv
                            join FP in _contexto.formaPago on C.CompFormaPago equals FP.FrPID
                            join CD in _contexto.comprasDet on C.CompID equals CD.CompDetCompras
                            where (T.TerNombre.Contains(filter) || T.TerID.ToString().Contains(filter))
                            && C.CompSuc.Equals(int.Parse(suc)) && (C.CompEstado.Equals(1) || C.CompEstado.Equals(2))
                            select T).ToList().GroupBy(x => x.TerID).Select(x => x.First()).ToList();
            }

            return terceros;
        }

        public List<ComprasDTO> ComprasProveedor(string proveedor, string suc)
        {
            List<ComprasDTO> compras = new List<ComprasDTO>();

            if (!string.IsNullOrEmpty(proveedor) && !string.IsNullOrEmpty(suc))
            {
                compras = (from C in _contexto.compras
                           join FP in _contexto.formaPago on C.CompFormaPago equals FP.FrPID
                           join M in _contexto.monedas on C.CompMoneda equals M.MonID
                           where C.CompProv.Equals(int.Parse(proveedor)) && C.CompSuc.Equals(int.Parse(suc))
                           && (C.CompEstado.Equals(1) || C.CompEstado.Equals(2))

                           select new ComprasDTO()
                           {
                               compra = new Compras()
                               {
                                   CompID = C.CompID,
                                   CompNo = C.CompNo,
                                   CompFecha = C.CompFecha,
                                   CompEstado = C.CompEstado,
                                   CompTotalPagar = C.CompTotalPagar,
                                   CompTotalPagarMM = C.CompTotalPagarMM
                               },
                               moneda = new Monedas()
                               {
                                   MonSimbolo = M.MonSimbolo,
                                   MonDesc = M.MonDesc,
                                   MonAbrev = M.MonAbrev
                               }
                           }).ToList() ?? new List<ComprasDTO>();
            }
            return compras;
        }

        public DetalllesOCEADTO ConsultaDetallesOC(string compra, string suc)
        {
            DetalllesOCEADTO dataRespuesta = new DetalllesOCEADTO();
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            try
            {

                parametros.Add("IdCompra", compra);
                parametros.Add("IdSucursal", suc);

                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor, new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_API_EA].[ConsultaDetallesOC]",
                    parametros = parametros
                });

                var datosEncabezado = (from dataLiq in resultado.AsEnumerable()
                                       select new EntradaAlmacenDTO()
                                       {
                                           entrada = new ADPEntradasAlmacen()
                                           {
                                               EnAID = (int)dataLiq["IdEntrada"],
                                               EnANo = (int)dataLiq["IdEntrada"],
                                               EnAFecha = DateTime.Parse((string)dataLiq["EnaFecha"]),
                                               EnAFechaFac = DateTime.Parse((string)dataLiq["EnaFecha"]),
                                               EnAFechaReciboNo = DateTime.Parse((string)dataLiq["EnaFecha"]),
                                               EnAObra = (int)dataLiq["ObrObra"]
                                           },
                                           compra = new ComprasDTO()
                                           {
                                               compra = new Compras()
                                               {
                                                   CompID = (int)dataLiq["CompId"],
                                                   CompNo = (int)dataLiq["CompNo"],
                                                   CompObs = (string)dataLiq["CompObs"],
                                                   CompSitioEnt = (string)dataLiq["CompSitioEnt"],
                                                   CompDesc = (string)dataLiq["CompDesc"],
                                                   CompProv = (int)dataLiq["CompProv"],
                                                   CompTotalPagar = (decimal)dataLiq["CompTotalPagarMM"],
                                                   CompFechaReq = DateTime.Parse((string)dataLiq["CompFechaReq"]),
                                                   CompMonedaTC = (decimal)dataLiq["CompMonedaTC"]
                                               },
                                               sucursal = new Sucursal()
                                               {
                                                   SucID = (short)dataLiq["CompSuc"],
                                                   SucDesc = (string)dataLiq["SucDesc"]
                                               },
                                               EstadoOrden = new EstadoOrdenCompraDTO()
                                               {
                                                   EOrID = (byte)dataLiq["CompEstado"],
                                                   EOrDesc = (string)dataLiq["EOrDesc"]
                                               },
                                               moneda = new Monedas()
                                               {
                                                   MonDesc = (string)dataLiq["MonDesc"],
                                                   MonAbrev = (string)dataLiq["MonAbrev"]
                                               },
                                               terceros = _contexto.tercero.Find((int)dataLiq["CompProv"])

                                           }
                                       }).FirstOrDefault();

                dataRespuesta.Encabezado = datosEncabezado;

                if (dataRespuesta.Encabezado != null)
                {
                    var datosDetalles = (from data in resultado.AsEnumerable()
                                         select new MovimientosInvDTO()
                                         {
                                             movimientosInv = new MovimientosInv()
                                             {
                                                 MvIID = (int)data["DetEA"],
                                                 MvIVrUnitMM = (decimal)data["CompDetUnitarioMM"],
                                                 MvICant = (decimal)data["CantDetEA"],
                                                 MvIVrTotalMM = (decimal)data["VrTotalEA"]
                                             },
                                             comprasDet = new ComprasDetDTO()
                                             {
                                                 comprasDet = new ComprasDet()
                                                 {
                                                     CompDetID = (int)data["CompDetID"],
                                                     CompDetCompras = (int)data["CompId"],
                                                     CompDetFechaReq = DateTime.Parse((string)data["CompDetFechaReq"]),
                                                     CompDetCant = (decimal)data["CompDetCant"],
                                                     CompDetUnitarioMM = (decimal)data["CompDetUnitarioMM"],
                                                     CompDetIVA = (decimal)data["CompDetIVA"],
                                                     CompDetBaseIvaDiff = (decimal)data["CompDetBaseIvaDiff"],
                                                     CompDetBaseIvaDiff2 = (decimal)data["CompDetBaseIvaDiff2"]
                                                 },
                                                 producto = new Producto()
                                                 {
                                                     ProCod = (int)data["ProCod"],
                                                     ProDesc = (string)data["ProDesc"],
                                                     ProUnidadCont = (string)data["ProUnidadCont"],
                                                     ProStockMinimo = (decimal)data["ProStockMinimo"],
                                                     ProCodBIM = (string)data["ProCodBIM"]

                                                 },

                                                 BIDIFF = Convert.ToBoolean((int)data["BIDIFF"])
                                             },
                                             DevAsociada = (int)data["DevAsoc"],
                                             CantidadPendiente = (decimal)data["CantPendiente"],
                                             CantMax = (decimal)data["CantMax"],
                                             CantMin = (decimal)data["CantMin"],
                                             CantInv = (decimal)data["CantInv"]

                                         }).ToList();

                    dataRespuesta.Detalles = datosDetalles;
                }
                else
                {
                    dataRespuesta.Detalles = new List<MovimientosInvDTO>();
                }

            }
            catch (Exception e)
            {
                dataRespuesta.Encabezado = new EntradaAlmacenDTO();
                dataRespuesta.Detalles = new List<MovimientosInvDTO>();
            }

            return dataRespuesta;
        }
    }
}
