using Inventarios.DTO;
using Model.DTO.ComprasD;
using Model.DTO.Inventarios;
using Model.Entity.DBO;
using Services.Inventarios;
using System.Text;
using System.Web;
using System.Xml;

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

            app.MapGet("/Entradas/Bodega", (IEntradasService _entradasService, string suc, string usuario) =>
            {
                try
                {
                    List<BodegasSucursalDTO> bodegas = _entradasService.ConsultaBodegas(suc, usuario);
                    return Results.Ok(bodegas);
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
                    List<TercerosDTO> _config = _entradasService.TercerosEntradas(filter, suc);
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
                    List<CompraDTO> _config = _entradasService.ComprasProveedor(proveedor, suc);
                    return Results.Ok(_config);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/ComprasPendientesXSuc", (IEntradasService _entradasService, string suc) =>
            {
                try
                {
                    List<PendienteEntradaDTO> _config = _entradasService.ComprasPendientesXSuc(suc);
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

            app.MapPut("/Entradas/Guarda", (IEntradasService _entradasService, EntradaAlmacenDTO data) =>
            {
                try
                {

                    data.EnAReciboNo = HttpUtility.UrlDecode(data.EnAReciboNo);
                    var mov = data.movimientosInv;
                    var encoding = Encoding.GetEncoding("ISO-8859-1");
                    System.Xml.Serialization.XmlSerializer Encabezado = new System.Xml.Serialization.XmlSerializer(data.GetType()); ;
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        OmitXmlDeclaration = false,
                        Encoding = encoding
                    };

                    using (var stream = new MemoryStream())
                    {
                        using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            Encabezado.Serialize(xmlWriter, data);
                        }

                        EntradaAlmacenTableDTO response = _entradasService.GuardarEntrada(data, encoding.GetString(stream.ToArray()));
                        return Results.Ok(response);
                    }

                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/ListadoEntradasEdicion", (IEntradasService _entradasService, int suc, string oc, int usu, int prov, int estado, string fechai, string fechaf, string ea) =>
            {
                try
                {
                    List<ListaEntradaAlmacenDTO> listaEntradas = _entradasService.ListadoEntradasEdicion(suc, oc, usu, prov, estado, fechai, fechaf, ea);
                    return Results.Ok(listaEntradas);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");

            app.MapGet("/Entradas/ConsultaEntradaAlmacen", (IEntradasService _entradasService, string idea, string suc) =>
            {
                try
                {
                    DetalllesOCEADTO entrada = _entradasService.ConsultaEntradaAlmacen(idea, suc);
                    return Results.Ok(entrada);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Entradas");
        }
    }
}
