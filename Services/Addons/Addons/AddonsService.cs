
using Repository.DataBase.Conexion;
using Model.DTO.Addons;
using Model.DTO;
using Model.Entity.Addons;
using Model.Entity.ADP_PNL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace Addons
{
    public interface IAddonServices
    {

        IEnumerable<AddonsDTO> GetAddons(int modulo);
        AddonConfigDTO GetAddon(int id);
        AddonConfigDTO GuardarAddon(AddonConfigDTO data);
        int calcularNumeroAddon(bool isCritico);

        IEnumerable<AutocompleteDTO> GetMenus(string filter = "");
        IEnumerable<AutocompleteDTO> GetConfig(string filter = "");

        IEnumerable<AutocompleteDTO> GetInformePanel(string filter = "");

    }

    public class AddonsService : IAddonServices
    {

        private DB_ADPRO _contexto;

        public AddonsService(DB_ADPRO contexto)
        {
            _contexto = contexto;
        }

        public AddonConfigDTO GetAddon(int id)
        {
            AddonConfigDTO objresultado = new AddonConfigDTO();

            objresultado.encabezado = _contexto.addonsListado.Where(c => c.IdListado == id).Select(c =>
                new EncabezadoAddonDTO()
                {
                    id = c.IdListado,
                    addonNombre = c.AddonNombre,
                    addonNo = c.AddonNumero,
                    obsoleto = c.Obsoleto,
                    fechaPublicacion = c.FechaPublicacion ?? DateTime.Now,
                    horasBdAdicional = c.HorasBdAdicional,
                    horasInstalacion = c.HorasInstalacion,
                    obs = c.Obs,
                    requisito = c.Requisitos,
                    rutaPDF = c.RutaPDF,
                    critico = c.AddonNumero <= -999,
                    costo = c.costo ?? 0,
                    estandar = c.Estandar ?? true,
                    modulo = c.Modulo

                }
            ).FirstOrDefault() ?? new EncabezadoAddonDTO() { };


            //detalles
            objresultado.detalle = (_contexto.addonsconfig.Where(c => c.IdListado == id)).Select(c => new DetalleAddonDTO()
            {
                id = c.IdConfig,
                addonNo = objresultado.encabezado.addonNo,
                anclaValor = c.AnclarVarlor,
                codigoNombre = c.ColCodigoNombre,
                codigoValor = c.ColCodigoValor,
                valorNombre = c.ColValorNombre,
                obs = c.Obs,
                valorValor = c.ColValorValor,
                tabla = c.Tabla

            }).ToList();

            return objresultado;
        }

        public IEnumerable<AddonsDTO> GetAddons(int modulo)
        {

            return _contexto.addonsListado.Where(c => c.Modulo == modulo).Select(c =>
                new AddonsDTO()
                {
                    Id = c.IdListado,
                    Nombre = c.AddonNombre,
                    Numero = c.AddonNumero.ToString(),
                    Obsoleto = c.Obsoleto,
                    Critico = c.AddonNumero <= -999,
                    IsEstandar = c.Estandar ?? true,
                    ModuloRequisito = c.ModuloRequisito ?? ""
                }
            ).ToList();
        }
        public int calcularNumeroAddon(bool isCritico)
        {
            int numero = 0;

            if (isCritico)
                numero = _contexto.addonsListado.Min(c => c.AddonNumero) - 1;

            else
                numero = _contexto.addonsListado.Max(c => c.AddonNumero) + 1;


            return numero;
        }

        public AddonConfigDTO GuardarAddon(AddonConfigDTO data)
        {
            AddonConfigDTO objResponse = new AddonConfigDTO();
            int id = 0;
            int addonno = 0;

            if (data.encabezado.id > 0)
            {
                var addon = _contexto.addonsListado.Find(data.encabezado.id);

                if (addon != null)
                {

                    id = data.encabezado.id;
                    addonno = data.encabezado.addonNo;

                    addon.AddonNumero = data.encabezado.critico ? calcularNumeroAddon(true) : data.encabezado.addonNo;
                    addon.AddonNombre = data.encabezado.addonNombre;
                    addon.HorasBdAdicional = data.encabezado.horasBdAdicional;
                    addon.HorasInstalacion = data.encabezado.horasInstalacion;
                    addon.FechaPublicacion = data.encabezado.fechaPublicacion;
                    addon.RutaPDF = data.encabezado.rutaPDF;
                    addon.Obs = data.encabezado.obs;
                    addon.Obsoleto = data.encabezado.obsoleto;
                    addon.costo = data.encabezado.costo;
                    addon.Requisitos = data.encabezado.requisito;
                    _contexto.Entry(addon).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                }
                else
                {
                    AddonsListado newAddon = new AddonsListado();
                    newAddon.AddonNumero = data.encabezado.critico ? calcularNumeroAddon(true) : calcularNumeroAddon(false);
                    newAddon.AddonNombre = data.encabezado.addonNombre;
                    newAddon.HorasBdAdicional = data.encabezado.horasBdAdicional;
                    newAddon.HorasInstalacion = data.encabezado.horasInstalacion;
                    newAddon.FechaPublicacion = data.encabezado.fechaPublicacion;
                    newAddon.RutaPDF = data.encabezado.rutaPDF;
                    newAddon.Obs = data.encabezado.obs;
                    newAddon.Obsoleto = data.encabezado.obsoleto;
                    newAddon.costo = data.encabezado.costo;
                    newAddon.Requisitos = data.encabezado.requisito;


                    newAddon.Modulo = data.encabezado.modulo;

                    _contexto.addonsListado.Add(newAddon);

                    id = newAddon.IdListado;
                    addonno = newAddon.AddonNumero;

                }
                _contexto.SaveChanges();

                if (id > 0)
                    guardarDetalleAddon(data.detalle, id);

                if (id > 0)
                    objResponse = GetAddon(id);
            }

            _contexto.ExecuteStoreQuery(new Repository.DataBase.Model.ProcedureDTO()
            {
                commandText = "addons.Dtsxml",
                parametros = new Dictionary<string, object>()
            {
                {"@addon",addonno },{"@Ctrz",0 },{"@modulo",data.encabezado.modulo }
            }
            });

            return objResponse;
        }

        void guardarDetalleAddon(List<DetalleAddonDTO> detalle, int idAddon)
        {

            _contexto.addonsconfig.Where(c => c.IdListado == idAddon).ToList().ForEach(d =>
            {
                _contexto.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            });

            foreach (var item in detalle)
            {
                _contexto.addonsconfig.Add(new AddonsConfig()
                {
                    AnclarVarlor = item.anclaValor,
                    ColCodigoValor = item.codigoValor,
                    ColCodigoNombre = item.codigoNombre,
                    ColValorNombre = item.valorNombre,
                    ColValorValor = item.valorValor,
                    IdConfig = 0,
                    IdListado = idAddon,
                    Obs = item.obs,
                    Tabla = item.tabla


                });
            }

            if (detalle.Count > 0)
            {
                _contexto.SaveChanges();

            }
        }

        public IEnumerable<AutocompleteDTO> GetMenus(string filter = "")
        {
            if (filter == "_")
                filter = "";


            if (string.IsNullOrEmpty(filter))
            {
                return _contexto.menus
                  .Where(c => c.Modulo == 8 && c.Ubicacion != null && c.Ubicacion.Trim().Length > 0
                  && c.ActMenu == 1 && c.Descripcion.Trim() != "--"
                )
                  .Take(20)
                  .Select(c => new AutocompleteDTO()
                  {
                      id = c.IdMenu.ToString(),
                      valor = c.Ruta,
                      descripcion = c.Descripcion
                  }).ToList();
            }
            else
            {
                return _contexto.menus
                    .Where(c => c.Modulo == 8 && c.Ubicacion != null && c.Ubicacion.Trim().Length > 0
                    && c.ActMenu == 1 && c.Descripcion.Trim() != "--"
                    && (c.Descripcion.Trim().ToLower().Contains(filter.ToLower())
                                || c.Ruta.ToLower().Trim().Contains(filter.ToLower()))
                     )
                    .Take(20)
                    .Select(c => new AutocompleteDTO()
                    {
                        id = c.IdMenu.ToString(),
                        valor = c.Ruta,
                        descripcion = c.Descripcion
                    }).ToList();
            }


        }

        public IEnumerable<AutocompleteDTO> GetConfig(string filter = "")
        {
            if (filter == "_")
                filter = "";


            if (string.IsNullOrEmpty(filter))
            {
                return _contexto.adpconfig
                    .Take(20)
                    .Select(c => new AutocompleteDTO()
                    {
                        id = c.CnfCodigo,
                        valor = "",
                        descripcion = c.CnfDescripcion
                    });
            }
            else
            {
                return _contexto.adpconfig
                   .Where(c => c.CnfDescripcion.ToLower().Trim().Contains(filter)
                           || c.CnfValor.ToLower().Trim().Contains(filter))
                    .Take(20)
                   .Select(c => new AutocompleteDTO()
                   {
                       id = c.CnfCodigo,
                       valor = "",
                       descripcion = c.CnfDescripcion
                   });

            }
        }

        public IEnumerable<AutocompleteDTO> GetInformePanel(string filter = "")
        {
            if (filter == "_")
                filter = "";


            if (string.IsNullOrEmpty(filter))
            {
                return (from inf in _contexto.panelInforme
                        join clase in _contexto.claseInforme on inf.ClaseInf equals clase.PnlId
                        join tipo in _contexto.tipoInforme on clase.TipInfo equals tipo.TipoInfId
                        select new AutocompleteDTO()
                        {
                            id = inf.InfId.ToString(),
                            descripcion = $"{tipo.TipoInfDescripcion} / {clase.PnlDescripcion} / {inf.InfDesc}",
                            valor = inf.InfId.ToString()
                        })
                    .Take(20)
                   .ToList();
            }
            else
            {
                return (from inf in _contexto.panelInforme
                        join clase in _contexto.claseInforme on inf.ClaseInf equals clase.PnlId
                        join tipo in _contexto.tipoInforme on clase.TipInfo equals tipo.TipoInfId
                        where inf.InfDesc.ToLower().Trim().Contains(filter.ToLower())
                                || clase.PnlDescripcion.ToLower().Trim().Contains(filter.ToLower())
                        select new AutocompleteDTO()
                        {
                            id = inf.InfId.ToString(),
                            descripcion = $"{tipo.TipoInfDescripcion} / ${clase.PnlDescripcion} / {inf.InfDesc}",
                            valor = inf.InfId.ToString()
                        })
                    .Take(20)
                   .ToList();

            }
        }
    }
}
