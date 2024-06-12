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
            app.MapGet("addons/maestro/listado", (IAddonServices addon, int modulo) =>
            {

                return Results.Ok(addon.GetAddons(modulo));
            }).WithTags("Addons")
               .WithDescription(" Consulta todos los addons")
               .Produces<IEnumerable<AddonsDTO>>().WithOpenApi();


            app.MapGet("addons/", (IAddonServices addon, int id) =>
            {

                return Results.Ok(addon.GetAddon(id));
            }).WithTags("Addons")
             .WithDescription(" Consulta todos los addons")
             .Produces<EncabezadoAddonDTO>().WithOpenApi();


            app.MapGet("addons/calcularnumero", (IAddonServices addon, bool isCritico) =>
            {

                return Results.Ok(new
                {
                    addonNo = addon.calcularNumeroAddon(isCritico)
                });
            }).WithTags("Addons")
                   .WithDescription("Genera el número del addon")
                   .Produces<int>().WithOpenApi();


            app.MapPost("addons/", (IAddonServices addon, AddonConfigDTO request) =>
            {

                return Results.Ok(addon.GuardarAddon(request));
            }).WithTags("Addons")
                  .WithDescription(" Consulta todos los addons")
                  .Produces<ResponseEstandarDTO>().WithOpenApi();



            app.MapGet("addons/menu", (IAddonServices addon, string filter = "") =>
            {

                return Results.Ok(addon.GetMenus(filter));
            }).WithTags("Addons")
                .WithDescription("Consulta los menus para configurar addons")
                .Produces<IEnumerable<MenuDTO>>().WithOpenApi();


            app.MapGet("addons/config", (IAddonServices addon, string filter = "") =>
            {

                return Results.Ok(addon.GetConfig(filter));
            }).WithTags("Addons")
                  .WithDescription("Consulta los config de ADPRO para configurar addons")
                  .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();


            app.MapGet("addons/informespanel", (IAddonServices addon, string filter = "") =>
            {
                return Results.Ok(addon.GetInformePanel(filter));
            }).WithTags("Addons")
                    .WithDescription("Consulta los Informes panel de ADPRO para configurar addons")
                    .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();


            app.MapPost("addons/solicitar", (IAddonServices addon, RequestHDSolicitudDTO request) =>
            {
                return Results.Ok(addon.SolicitudHD(request));
            }).WithTags("Addons")
                   .WithDescription("Generar un HD para la instalacion del addon")
                   .Produces<IEnumerable<AutocompleteDTO>>().WithOpenApi();

        }
    }
}
