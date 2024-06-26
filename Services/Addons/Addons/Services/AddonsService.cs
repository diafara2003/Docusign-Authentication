
using Repository.DataBase.Conexion;
using Model.DTO.Addons;
using Model.Entity.Addons;
using Microsoft.EntityFrameworkCore;
using Addons.DTO;
using HandleError;
using Model.DTO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Addons.Services
{
    public interface IAddonServices
    {

        IEnumerable<AddonsDTO> GetAddons(int modulo);
        AddonConfigDTO GetAddon(int id);
        AddonConfigDTO GuardarAddon(AddonConfigDTO data);
        int calcularNumeroAddon(bool isCritico, int id = 0, bool publicar = false);
        IEnumerable<AutocompleteDTO> GetMenus(int modulo = 0, string filter = "");
        IEnumerable<AutocompleteDTO> GetConfig(string filter = "");
        IEnumerable<AutocompleteDTO> GetInformePanel(string filter = "");
        ResponseEstandarDTO PublicarAddon(PublicarAddonDTO request);
        ResponseAPIDTO EliminarAddon(int id);
        IEnumerable<AutocompleteDTO> GetModulos(int modulo = 0, string filter = "");
        IEnumerable<AutocompleteDTO> GetAddonsAC(int modulo = 0, string filter = "");
        ResponseEstandarDTO MarcarObsoletoAddon(MarcarObsoletoAddonDTO request);
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
                    modulo = c.Modulo,
                    instalable = c.Instalar,
                    visible = c.Ver,
                    requisitoModulo = c.ModuloRequisito ?? ""

                }
            ).FirstOrDefault() ?? new EncabezadoAddonDTO() { };


            //detalles
            objresultado.detalle = _contexto.addonsconfig.Where(c => c.IdListado == id).Select(c => new DetalleAddonDTO()
            {
                id = c.IdConfig,
                addonNo = objresultado.encabezado.addonNo,
                anclaValor = c.AnclarVarlor,
                codigoNombre = c.ColCodigoNombre,
                codigoValor = c.ColCodigoValor,
                valorNombre = c.ColValorNombre,
                obs = c.Obs,
                valorValor = c.ColValorValor,
                tabla = c.Tabla,
                menuOld = string.IsNullOrEmpty(c.MenuOld) ? string.Empty : c.MenuOld

            }).ToList();

            objresultado.detalle
                .ForEach(c =>
                {
                    if (string.IsNullOrEmpty(c.menuOld))
                    {
                        return;
                    }

                    if (c.menuOld.Contains(","))
                    {
                        c.MenuOldDesc = "Multiples menús";
                    }
                    else
                    {
                        var _menu = _contexto.menus.Find(int.Parse(c.menuOld));

                        if (_menu != null)
                        {
                            c.MenuOldDesc = _menu.Ubicacion;

                        }
                    }

                });


            return objresultado;
        }

        public IEnumerable<AddonsDTO> GetAddons(int modulo)
        {

            return _contexto.addonsListado.Where(c => c.Modulo == modulo)
                .OrderBy(c => c.AddonNumero)
                .Select(c =>
                new AddonsDTO()
                {
                    Id = c.IdListado,
                    Nombre = c.AddonNombre,
                    Numero = c.AddonNumero.ToString(),
                    Obsoleto = c.Obsoleto,
                    Critico = c.AddonNumero <= -999,
                    IsEstandar = c.Estandar ?? true,
                    ModuloRequisito = c.ModuloRequisito ?? "",
                    addonRequisito = c.Requisitos ?? "",
                    publicado = c.AddonNumero > 0 || c.AddonNumero < -999 && c.Ver

                }
            ).ToList();
        }
        public int calcularNumeroAddon(bool isCritico, int id = 0, bool publicar = false)
        {
            int numero = 0;

            if (publicar == true)
            {
                if (isCritico)
                {
                    numero = _contexto.addonsListado.Min(c => c.AddonNumero) - 1;

                    return (numero += 1000) * -1;
                }
                else
                {
                    return _contexto.addonsListado.Where(c => c.AddonNumero > -1000).Max(c => c.AddonNumero) + 1;
                }
            }

            if (isCritico)
            {

                if (id > 0)
                {
                    numero = (_contexto.addonsListado.Find(id) ?? new AddonsListado() { AddonNumero = 0 }).AddonNumero;
                }
                else
                {
                    numero = _contexto.addonsListado.Min(c => c.AddonNumero) - 1;
                }
                numero += 1000;

                numero = numero * -1;

            }
            else
            {
                if (id > 0)
                {
                    numero = (_contexto.addonsListado.Find(id) ?? new AddonsListado() { AddonNumero = 0 }).AddonNumero;
                }
                else
                {
                    numero = _contexto.addonsListado.Where(c => c.AddonNumero > -1000).Min(c => c.AddonNumero) - 1;
                }
            }

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

                int numero = addon.AddonNumero;

                if (addon != null)
                {

                    id = data.encabezado.id;

                    bool isCriticoCurrent = addon.AddonNumero < -999;

                    if (isCriticoCurrent != data.encabezado.critico)
                    {
                        numero = calcularNumeroAddon(data.encabezado.critico, id, false);


                        if (data.encabezado.critico)
                        {
                            numero += -1000;
                        }
                    }

                    addon.AddonNumero = numero;
                    addon.AddonNombre = data.encabezado.addonNombre;
                    addon.HorasBdAdicional = data.encabezado.horasBdAdicional;
                    addon.HorasInstalacion = data.encabezado.horasInstalacion;
                    addon.FechaPublicacion = data.encabezado.fechaPublicacion;
                    addon.RutaPDF = data.encabezado.rutaPDF;
                    addon.Obs = data.encabezado.obs;
                    addon.Obsoleto = data.encabezado.obsoleto;
                    addon.costo = data.encabezado.costo;
                    addon.Requisitos = data.encabezado.requisito;
                    addon.ModuloRequisito = data.encabezado.requisitoModulo;
                    addon.Instalar = data.encabezado.instalable;
                    addon.Ver = data.encabezado.critico ? false : data.encabezado.visible;

                    _contexto.Entry(addon).State = EntityState.Modified;

                    _contexto.SaveChanges();
                    addonno = addon.AddonNumero;
                }
            }
            else
            {

                int numero = calcularNumeroAddon(data.encabezado.critico, 0);

                if (data.encabezado.critico)
                {
                    numero += -1000;
                }


                AddonsListado newAddon = new AddonsListado();
                newAddon.AddonNumero = numero;//; data.encabezado.critico ? -1000 + data.encabezado.addonNo * -1 : data.encabezado.addonNo * -1;
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
                newAddon.Estandar = data.encabezado.estandar;
                newAddon.SubModulo = "Página";
                newAddon.Instalar = data.encabezado.instalable;
                newAddon.Ver = data.encabezado.visible;
                newAddon.ModuloRequisito = data.encabezado.requisitoModulo;
                //newAddon.Publicado = false;

                _contexto.addonsListado.Add(newAddon);

             _contexto.SaveChanges();
                id = newAddon.IdListado;
                addonno = newAddon.AddonNumero;

            }

            if (id > 0)
                guardarDetalleAddon(data.detalle, id);

            if (id > 0)
                objResponse = GetAddon(id);

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
                _contexto.Entry(d).State = EntityState.Deleted;
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
                    Tabla = item.tabla,
                    MenuOld = item.menuOld
                });
            }

            if (detalle.Count > 0)
            {
                _contexto.SaveChanges();

            }
        }

        public IEnumerable<AutocompleteDTO> GetMenus(int modulo = 0, string filter = "")
        {
            if (filter == "_")
                filter = "";


            if (string.IsNullOrEmpty(filter))
            {
                return _contexto.menus
                  .Where(c => c.Modulo == modulo && c.Ubicacion != null && c.Ubicacion.Trim().Length > 0
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
                    .Where(c => c.Modulo == modulo && c.Ubicacion != null && c.Ubicacion.Trim().Length > 0
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
                        valor = c.CnfDescripcion,
                        descripcion = c.CnfCodigo
                    });
            }
            else
            {
                return _contexto.adpconfig
                   .Where(c => c.CnfDescripcion.ToLower().Trim().Contains(filter)
                           || c.CnfCodigo.ToLower().Trim().Contains(filter))
                    .Take(20)
                   .Select(c => new AutocompleteDTO()
                   {
                       id = c.CnfCodigo,
                       valor = c.CnfDescripcion,
                       descripcion = c.CnfCodigo
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
                    .Take(40)
                   .ToList();
            }
            else
            {
                return (from inf in _contexto.panelInforme
                        join clase in _contexto.claseInforme on inf.ClaseInf equals clase.PnlId
                        join tipo in _contexto.tipoInforme on clase.TipInfo equals tipo.TipoInfId
                        where inf.InfDesc.ToLower().Trim().Contains(filter.ToLower())
                        || tipo.TipoInfDescripcion.ToLower().Trim().Contains(filter.ToLower())
                        || clase.PnlDescripcion.ToLower().Trim().Contains(filter.ToLower())

                        select new AutocompleteDTO()
                        {
                            id = inf.InfId.ToString(),
                            descripcion = $"{tipo.TipoInfDescripcion} / {clase.PnlDescripcion} / {inf.InfDesc}",
                            valor = inf.InfId.ToString()
                        })
                    .Take(40)
                   .ToList();

            }
        }

        public ResponseEstandarDTO PublicarAddon(PublicarAddonDTO request)
        {

            ResponseEstandarDTO objResponse = new ResponseEstandarDTO();
            objResponse.success = true;

            var addon = _contexto.addonsListado.Find(request.id);

            if (addon != null)
            {
                addon.Instalar = request.instalable;

                if (addon.AddonNumero < -999)
                {
                    request.numero = (request.numero += 1000) * -1;
                }

                addon.AddonNumero = request.numero; //_contexto.addonsListado.Max(c => c.AddonNumero) + 1;
                                                    // addon.Publicado = true;

                objResponse.codigo = addon.AddonNumero;

                _contexto.SaveChanges();
            }

            return objResponse;
        }

        public ResponseEstandarDTO MarcarObsoletoAddon(MarcarObsoletoAddonDTO request)
        {

            ResponseEstandarDTO objResponse = new ResponseEstandarDTO();
            objResponse.success = true;

            var addon = _contexto.addonsListado.Find(request.id);

            if (addon != null)
            {
                addon.Obsoleto = true;
                // addon.Publicado = true;
                addon.Reemplazo = request.reemplazo;

                objResponse.codigo = addon.AddonNumero;

                _contexto.SaveChanges();
            }

            return objResponse;
        }

        public ResponseAPIDTO EliminarAddon(int id)
        {
            ResponseAPIDTO objResponse = new ResponseAPIDTO();
            var addon = _contexto.addonsListado.Find(id);

            if (addon != null)
            {
                _contexto.addonsconfig.Where(c => c.IdListado == id)
                    .ToList()
                    .ForEach(c =>
                    {
                        _contexto.Entry(c).State = EntityState.Deleted;
                    });

                _contexto.Entry(addon).State = EntityState.Deleted;

                _contexto.SaveChanges();
            }

            return objResponse;
        }

        public IEnumerable<AutocompleteDTO> GetModulos(int modulo = 0, string filter = "")
        {
            if (filter == "_")
                filter = "";
            List<AutocompleteDTO> objModulos = new List<AutocompleteDTO>();

            objModulos.Add(new AutocompleteDTO()
            {
                id = "GTH",
                valor = "",
                descripcion = "GTH"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "A&F",
                valor = "",
                descripcion = "A&F"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "M&E",
                valor = "",
                descripcion = "M&E"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "CBR",
                valor = "",
                descripcion = "CBR"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "F&C",
                valor = "",
                descripcion = "F&C"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "ABR",
                valor = "",
                descripcion = "ABR"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "SGC",
                valor = "",
                descripcion = "SGC"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "SGD",
                valor = "",
                descripcion = "SGD"
            });
            objModulos.Add(new AutocompleteDTO()
            {
                id = "SRM",
                valor = "",
                descripcion = "SRM"
            });



            if (string.IsNullOrEmpty(filter) == false)
            {
                return objModulos
                  .Where(c => c.descripcion.ToLower().Trim().Contains(filter))
                   .Take(20)
                   .ToList();

            }
            else
            {
                return objModulos;

            }
        }

        public IEnumerable<AutocompleteDTO> GetAddonsAC(int modulo = 0, string filter = "")
        {
            if (filter == "_")
                filter = "";

            if (string.IsNullOrEmpty(filter))
            {
                return _contexto.addonsListado
                    .Where(c => c.Obsoleto == false && c.AddonNumero > 0)

                    .Select(c => new AutocompleteDTO()
                    {
                        id = c.IdListado.ToString(),
                        valor = "",
                        descripcion = $"{(c.AddonNumero < -1000 ? "" : $"{c.AddonNumero} -")} {c.AddonNombre}"
                    });
            }
            else
            {
                return _contexto.addonsListado
                   .Where(c => c.AddonNumero.ToString().Trim().Contains(filter.ToLower())
                           || c.AddonNombre.ToLower().Trim().Contains(filter.ToLower()))

                   .Select(c => new AutocompleteDTO()
                   {
                       id = c.IdListado.ToString(),
                       valor = "",
                       descripcion = $"{(c.AddonNumero < -1000 ? "" : $"{c.AddonNumero} -")} - {c.AddonNombre}"
                   });

            }
        }
    }
}

