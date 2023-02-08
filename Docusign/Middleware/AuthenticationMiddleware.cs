using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Repository.DataBase.Conexion;
using SincoSoft.Context.Core;
using System;
using System.Threading.Tasks;
using Utilidades.Session;

namespace Docusign.Middleware
{

    public class AuthenticationMiddleware
    {
        readonly RequestDelegate next;
        readonly IConfiguration configuration;
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IConstruirSession construirSession;

        public AuthenticationMiddleware(RequestDelegate next
        , IConfiguration _configuration
        , IHttpContextAccessor _httpContextAccessor
        , Utilidades.Session.IConstruirSession _construirSession
        )
        {
            this.next = next;
            configuration = _configuration;
            httpContextAccessor = _httpContextAccessor;
            construirSession = _construirSession;
        }
        public async Task Invoke(HttpContext context)
        {
            string urlPeticion = context.Request.GetDisplayUrl().ToLower();
            //if (urlPeticion.Contains("/api/"))
            //{

            //ObtenerSesion(context);


            
            //new DB_ADPRO(construirSession, httpContextAccessor);
            
            await next(context);
        }
        private void ObtenerSesion(HttpContext context)
        {
            int IdUsuario = 50;
            int IdEmpresa = 1;
            int IdSucursal = 0;
            string cadenaConexion;
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
            try
            {
                CurrentContext contextoActual = new CurrentContext(httpContextAccessor);
                IdUsuario = Convert.ToInt32(contextoActual.IdUsuario.ToString());
                IdEmpresa = Convert.ToInt32(contextoActual.IdEmpresa.ToString());
                IdSucursal = Convert.ToInt32(contextoActual.IdSucursal.ToString());
                cadenaConexion = contextoActual.CadenaConexion;
            }
            catch
            {
                throw new Exception("Token invalido");
            }
            //}
            context.Items["_cadenaConexion"] = cadenaConexion;
            context.Items["_idEmpresa"] = IdEmpresa;
            context.Items["_idSucursal"] = IdSucursal;
            context.Items["_idUsuario"] = IdUsuario;
        }
    }


}
