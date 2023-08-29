using Model.DTO.ComprasD;
using Model.DTO.Inventarios;
using Model.Entity.DBO;
using Services.Inventarios;

namespace API.Routes.MapInventarios
{
    public static class MapEntradasRoutes
    {
        public static void RegisterInventarios(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Entradas/Config", (IEntradasService _entradasService) =>
            {
                try
                {
                    List<ADPConfig> _config = _entradasService.ConfigEntradas();
                    return Results.Ok(_config);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/Proveedor", (IEntradasService _entradasService, string filter, string suc) =>
            {
                try
                {
                    List<Terceros> _config = _entradasService.TercerosEntradas(filter, suc);
                    return Results.Ok(_config);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/ComprasProveedor", (IEntradasService _entradasService, string proveedor, string suc) =>
            {
                try
                {
                    List<ComprasDTO> _config = _entradasService.ComprasProveedor(proveedor, suc);
                    return Results.Ok(_config);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/DetallesOC", (IEntradasService _entradasService, string compra, string suc) =>
            {
                try
                {
                    DetalllesOCEADTO _config = _entradasService.ConsultaDetallesOC(compra, suc);
                    return Results.Ok(_config);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");
        }
    }
}
