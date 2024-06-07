using Autodesk.Forge.Model;
using Repository.DataBase.Conexion;
using Inventarios.DTO;
using Microsoft.AspNetCore.Http;
using Model.DTO;
using Model.DTO.ComprasD;
using Model.DTO.Inventarios;
using Model.Entity.DBO;
using Repository.DataBase.Conexion;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Services.Inventarios
{
    public interface IEntradasService
    {
        List<ADPConfig> ConfigEntradas();
        List<BodegasSucursalDTO> ConsultaBodegas(string suc, string usuario);
        List<TercerosDTO> TercerosEntradas(string filter, string suc);
        List<CompraDTO> ComprasProveedor(string proveedor, string suc);
        List<PendienteEntradaDTO> ComprasPendientesXSuc(string suc);
        DetalllesOCEADTO ConsultaDetallesOC(string compra, string suc);
        EntradaAlmacenTableDTO GuardarEntrada(EntradaAlmacenDTO data, string XmlEncabezadoEA);
        List<ListaEntradaAlmacenDTO> ListadoEntradasEdicion(int suc, string oc, int usu, int prov, int estado, string fechai, string fechaf, string ea);
        DetalllesOCEADTO ConsultaEntradaAlmacen(string idea, string suc);
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


        public List<TercerosDTO> TercerosEntradas(string filter, string suc)
        {
            List<TercerosDTO> terceros = new List<TercerosDTO>();

            if (!string.IsNullOrEmpty(suc))
            {

                if (filter == "_")
                {
                    filter = " ";
                }

                terceros = (from T in _contexto.tercero
                            join C in _contexto.compras on T.TerID equals C.CompProv
                            join FP in _contexto.formaPago on C.CompFormaPago equals FP.FrPID
                            join CD in _contexto.comprasDet on C.CompID equals CD.CompDetCompras
                            where (T.TerNombre.Contains(filter) || T.TerID.ToString().Contains(filter) || (T.TerID.ToString() + " - " + T.TerNombre).Contains(filter))
                                   && (C.CompEstado.Equals(1) || C.CompEstado.Equals(2)) && C.CompSuc.Equals(Convert.ToInt16(suc))
                            select new TercerosDTO()
                            {
                                id = T.TerID,
                                nombre = T.TerNombre,
                            }).ToList().GroupBy(x => x.id).Select(x => x.First()).ToList();
            }

            return terceros;
        }

        public List<CompraDTO> ComprasProveedor(string proveedor, string suc)
        {
            List<CompraDTO> compras = new List<CompraDTO>();

            if (!string.IsNullOrEmpty(proveedor) && !string.IsNullOrEmpty(suc))
            {
                compras = (from C in _contexto.compras
                           join FP in _contexto.formaPago on C.CompFormaPago equals FP.FrPID
                           join M in _contexto.monedas on C.CompMoneda equals M.MonID
                           where C.CompProv.Equals(int.Parse(proveedor)) && C.CompSuc.Equals(Convert.ToInt16(suc))
                           && (C.CompEstado.Equals(1) || C.CompEstado.Equals(2))

                           select new CompraDTO()
                           {
                                CompID = C.CompID,
                                CompTotalPagar = C.CompTotalPagar,
                                CompTotalPagarMM = C.CompTotalPagarMM,
                                MonSimbolo = M.MonSimbolo,
                                MonAbrev = M.MonAbrev
                           }).ToList() ?? new List<CompraDTO>();
            }
            return compras;
        }

        public List<PendienteEntradaDTO> ComprasPendientesXSuc(string suc)
        {
            List<PendienteEntradaDTO> compras = new List<PendienteEntradaDTO>();

            if (!string.IsNullOrEmpty(suc))
            {
                compras = (from C in _contexto.compras
                           join T in _contexto.tercero on C.CompProv equals T.TerID
                           where C.CompSuc.Equals(Convert.ToInt16(suc))
                           && (C.CompEstado.Equals(1) || C.CompEstado.Equals(2))

                           select new PendienteEntradaDTO()
                           {
                               EnASuc = Convert.ToInt32(C.CompSuc),
                               NombreSuc = "",
                               EnAOC = C.CompID,
                               TerNombre = T.TerNombre,
                               TerID = C.CompProv,
                               NoEntradas = (from E in _contexto.adpEntradasAlmacen where E.EnAOC.Equals(C.CompID) select E.EnAID).Count(),
                               FechaUltimaEntrada = (from E in _contexto.adpEntradasAlmacen where E.EnAOC.Equals(C.CompID) select E.EnAFecha).Max().ToString()
                           }).ToList() ?? new List<PendienteEntradaDTO>();
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
                                       select new EntradaAlmacenTableDTO()
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
                dataRespuesta.Encabezado = new EntradaAlmacenTableDTO();
                dataRespuesta.Detalles = new List<MovimientosInvDTO>();
            }

            return dataRespuesta;
        }

        public EntradaAlmacenTableDTO GuardarEntrada(EntradaAlmacenDTO data, string XmlEncabezadoEA)
        {
            EntradaAlmacenTableDTO dataRespuesta = new EntradaAlmacenTableDTO();
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            try
            {
                parametros.Add("IdEntrada", data.EnAID);
                parametros.Add("IdSucursal", data.EnASuc);
                parametros.Add("IdCompra", data.EnAOC);
                parametros.Add("IdBodega", data.Bodega);
                parametros.Add("XmlEncabezadoEA", XmlEncabezadoEA);
                parametros.Add("IdDetOC", data.movimientosInv.MvICompDetID);
                parametros.Add("IdMov", data.movimientosInv.MvIID);
                parametros.Add("CantEA", data.movimientosInv.MvICant);
                parametros.Add("VrUnitEA", data.movimientosInv.MvIVrUnit);
                parametros.Add("SoloEncabezado", data.SoloEncabezado);
                parametros.Add("IdUsuario", data.EnAUsu);
                parametros.Add("FechaMin", data.FechaMin);
                parametros.Add("VrBaseIva", data.movimientosInv.MvIBaseIvaDiff);
                parametros.Add("VrBaseIva2", data.movimientosInv.MvIBaseIvaDiff2);

                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor, new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_API_EA].[GuardaEntradaAlmacen]",
                    parametros = parametros
                });

                ADPEntradasAlmacen entrada = new ADPEntradasAlmacen();
                entrada.EnAID = (int)resultado.Rows[0]["IdEntrada"];
                entrada.EnANo = (int)resultado.Rows[0]["NoEntrada"];

                ResponseV3DTO Respuesta = new ResponseV3DTO();
                Respuesta.codigo = (int)resultado.Rows[0]["Codigo"];
                Respuesta.mensaje = (string)resultado.Rows[0]["Mensaje"];
                Respuesta.OtroValor = Convert.ToString((int)resultado.Rows[0]["IdMov"]);

                dataRespuesta.entrada = entrada;
                dataRespuesta.Respuesta = Respuesta;
            }
            catch (Exception e)
            {
                dataRespuesta = new EntradaAlmacenTableDTO();
            }

            return dataRespuesta;
        }

        public List<ListaEntradaAlmacenDTO> ListadoEntradasEdicion(int suc, string oc, int usu, int prov, int estado, string fechai, string fechaf, string ea)
        {
            List<ListaEntradaAlmacenDTO> dataRespuesta = new List<ListaEntradaAlmacenDTO>();
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            try
            {
                parametros.Add("sucursal", suc);
                parametros.Add("OC", oc);
                parametros.Add("Usu", usu);
                parametros.Add("Proveedor", prov);
                parametros.Add("Estado", estado);
                parametros.Add("FechaInicial", fechai);
                parametros.Add("FechaFinal", fechaf);
                parametros.Add("EnANo", ea);


                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor, new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_API_EA].[ConsultaEntradasEditar]",
                    parametros = parametros
                });

                dataRespuesta = (from data in resultado.AsEnumerable()
                                 select new ListaEntradaAlmacenDTO()
                                 {
                                     EnAID = (int)data["EnAID"],
                                     EnASuc = Convert.ToInt32((Int16)data["EnASuc"]),
                                     Sucursal = (string)data["SucDesc"],
                                     EnAOC = (int)data["EnAOC"],
                                     EnANo = (int)data["EnANoInt"],
                                     EnAReciboNo = (string)data["EnAReciboNo"],
                                     TerNombre = (string)data["TerNombre"],
                                     EnAFecha = (string)data["EnAFecha"],
                                     MvIVrTotalMM = (string)data["MvIVrTotalMM"],
                                     EOrDesc = (string)data["EOrDesc"],
                                     configuracion = (string)data["configuracion"],
                                     CantEA = (decimal)data["MviCant"]
                                 }).ToList();

            }
            catch (Exception e)
            {
                dataRespuesta = new List<ListaEntradaAlmacenDTO>();
            }

            return dataRespuesta;
        }

        public DetalllesOCEADTO ConsultaEntradaAlmacen(string idea, string suc)
        {
            DetalllesOCEADTO dataRespuesta = new DetalllesOCEADTO();
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            try
            {

                parametros.Add("IdEntrada", idea);
                parametros.Add("IdSucursal", suc);

                var resultado = new DB_Execute().ExecuteStoreQuery(_httpContextAccessor, new Repository.DataBase.Model.ProcedureDTO()
                {
                    commandText = "[ADP_API_EA].[ConsultaEntradaAlmacen]",
                    parametros = parametros
                });

                var datosEncabezado = (from dataLiq in resultado.AsEnumerable()
                                       select new EntradaAlmacenTableDTO()
                                       {
                                           entrada = new ADPEntradasAlmacen()
                                           {
                                               EnAID = (int)dataLiq["EnAID"],
                                               EnANo = (int)dataLiq["EnANo"],
                                               EnAFecha = DateTime.Parse((string)dataLiq["EnaFecha"]),
                                               EnAFechaFac = DateTime.Parse((string)dataLiq["EnAFechaFac"]),
                                               EnAFechaReciboNo = DateTime.Parse((string)dataLiq["EnAFechaReciboNo"]),
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
                                                 MvIID = (int)data["MvIID"],
                                                 MvIVrUnitMM = (decimal)data["MvIVrUnitMM"],
                                                 MvICant = (decimal)data["MvICant"],
                                                 MvIVrTotalMM = (decimal)data["MvIVrTotalMM"]
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
                                             DevAsociada = (int)data["CantDevAsoc"],
                                             CantidadPendiente = (decimal)data["CantPendiente"],
                                             NotasAsociadas = (int)data["NotasAsociadas"],
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
                dataRespuesta.Encabezado = new EntradaAlmacenTableDTO();
                dataRespuesta.Detalles = new List<MovimientosInvDTO>();
            }

            return dataRespuesta;
        }
    }
}
