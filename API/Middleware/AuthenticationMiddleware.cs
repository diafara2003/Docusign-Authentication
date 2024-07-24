using Repository.DataBase.Conexion;
using Microsoft.AspNetCore.Http.Extensions;
using Repository.DataBase.Model;
using SincoSoft.Context.Core;
using HandleError;

namespace API.Middleware
{

    public class AuthenticationMiddleware
    {
        readonly RequestDelegate next;
        //readonly IConfiguration configuration;
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IConstruirSession construirSession;
        readonly IHandleError error;

        public AuthenticationMiddleware(RequestDelegate next
        //, IConfiguration _configuration
        , IHttpContextAccessor _httpContextAccessor
        , IConstruirSession _construirSession,
            IHandleError _error
        )
        {
            this.next = next;
            //configuration = _configuration;
            httpContextAccessor = _httpContextAccessor;
            construirSession = _construirSession;
            error = _error;
        }
        public async Task Invoke(HttpContext context)
        {
            string urlPeticion = context.Request.GetDisplayUrl().ToLower();

            try
            {
             

                if (urlPeticion.ToLower().Contains("weatherforecast")
                    || urlPeticion.ToLower().Contains("bim360")
                    || urlPeticion.ToLower().Contains("callback")
                    ) await next(context);
                else
                {
                    if (!urlPeticion.Contains("callback"))
                    {
                        ObtenerSesion(context);

                        new DB_ADPRO(construirSession, httpContextAccessor);
                    }
                    await next(context);
                }
            }
            catch (Exception e)
            {
                error.GuardarError(e, urlPeticion);
            }
          

            
        }
        private void ObtenerSesion(HttpContext context)
        {
            //int IdUsuario = 50;
            //int IdEmpresa = 1;
            //int IdSucursal = 0;
            //int IdObra = 0;
            //string cadenaConexion;
            //if (configuration["Enviroment"].ToString().Equals("Desarrollo"))
            //{
            //    cadenaConexion = configuration.GetConnectionString("DevelopConnection");
            //}
            //else
            //{

            string token = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("No se recibio el token de autenticación");

            }
            //try
            //{
            //    CurrentContext contextoActual = new CurrentContext(httpContextAccessor);
            //    IdUsuario = Convert.ToInt32(contextoActual.IdUsuario.ToString());
            //    IdEmpresa = Convert.ToInt32(contextoActual.IdEmpresa.ToString());
            //    IdSucursal = Convert.ToInt32(contextoActual.IdSucursal.ToString());
            //    IdObra = Convert.ToInt32(contextoActual.IdProyecto.ToString());
            //    cadenaConexion = contextoActual.CadenaConexion;
            //}
            //catch
            //{
            //    throw new Exception("Token invalido");
            //}
            ////}
            //context.Items["_cadenaConexion"] = cadenaConexion;
            //context.Items["_idEmpresa"] = IdEmpresa;
            //context.Items["_idSucursal"] = IdSucursal;
            //context.Items["_idUsuario"] = IdUsuario;
            //context.Items["_idObra"] = IdObra;
        }
    }


}
