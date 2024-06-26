using Addons.DTO;
using Addons.Services;
using Model.DTO;
using Model.DTO.Addons;

namespace API.Routes.MapAddons
{
    public static class MapAddonsRoutes
    {
        public static void RegisterAddons(this IEndpointRouteBuilder app)
        {

            app.MapPost("addons/", (IAddonServices addon, AddonConfigDTO request) => Results.Ok(addon.GuardarAddon(request))
         ).WithTags("Addons")
               .WithDescription(" Consulta todos los addons")
               .Produces<ResponseEstandarDTO>().WithOpenApi();

            app.MapGet("addons/", (IAddonServices addon, int id) => Results.Ok(addon.GetAddon(id))
            ).WithTags("Addons")
             .WithDescription(" Consulta todos los addons")
             .Produces<EncabezadoAddonDTO>().WithOpenApi();

            app.MapDelete("addons", (IAddonServices addon, int id) => Results.Ok(addon.EliminarAddon(id))
        ).WithTags("Addons")
             .WithDescription("Elimina la configuracion de los addons")
             .Produces<IEnumerable<ResponseAPIDTO>>().WithOpenApi();


            app.MapGet("addons/maestro/listado", (IAddonServices addon, int modulo) => Results.Ok(addon.GetAddons(modulo))
      ).WithTags("Addons")
         .WithDescription(" Consulta todos los addons")
         .Produces<IEnumerable<AddonsDTO>>().WithOpenApi();

            app.MapGet("addons/calcularnumero", (IAddonServices addon, int id, bool isCritico,bool publicar=false) =>
            {
                return Results.Ok(new
                {
                    addonNo = addon.calcularNumeroAddon(isCritico, id, publicar)
                });
            }).WithTags("Addons")
                   .WithDescription("Genera el número del addon")
                   .Produces<int>().WithOpenApi();


            app.MapGet("addons/menu", (IAddonServices addon, int modulo, string filter = "") => 
            Results.Ok(addon.GetMenus(modulo, filter))
            ).WithTags("Addons")
                .WithDescription("Consulta los menus para configurar addons")
                .Produces<IEnumerable<MenuDTO>>().WithOpenApi();


            app.MapGet("addons/config", (IAddonServices addon, string filter = "") => Results.Ok(addon.GetConfig(filter))
            ).WithTags("Addons")
                  .WithDescription("Consulta los config de ADPRO para configurar addons")
                  .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();


            app.MapGet("addons/informespanel", (IAddonServices addon, string filter = "") => Results.Ok(addon.GetInformePanel(filter))
            ).WithTags("Addons")
                    .WithDescription("Consulta los Informes panel de ADPRO para configurar addons")
                    .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();


            app.MapGet("addons/addonsAC", (IAddonServices addon, string filter = "") => Results.Ok(addon.GetAddonsAC(filter:filter))
         ).WithTags("Addons")
                 .WithDescription("Consulta los addons vigentes")
                 .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();

            app.MapGet("addons/modulosmenu", (IAddonServices addon, string filter = "") => Results.Ok(addon.GetModulos(filter: filter))
         ).WithTags("Addons")
                 .WithDescription("Consulta los modulos del sistema ")
                 .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();


            app.MapPost("addons/publicar", (IAddonServices addon, PublicarAddonDTO request) => Results.Ok(addon.PublicarAddon(request))
           ).WithTags("Addons")
                .WithDescription("Elimina la configuracion de los addons")
                .Produces<IEnumerable<ResponseAPIDTO>>().WithOpenApi();


            app.MapPost("addons/marcarobsoleto", (IAddonServices addon, MarcarObsoletoAddonDTO request) => Results.Ok(addon.MarcarObsoletoAddon(request))
        ).WithTags("Addons")
             .WithDescription("Marcar como obsoleto el addon o la actualizacion critica")
             .Produces<IEnumerable<ResponseAPIDTO>>().WithOpenApi();

        }
    }
}
