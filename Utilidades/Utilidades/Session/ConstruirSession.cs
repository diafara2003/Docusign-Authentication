using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;


namespace Docusign.Utilidades.Session
{

    public interface IConstruirSession
    {
        SessionDTO ObtenerSession();
        DbContextOptions ObtenerConextOptions(DbContextOptionsBuilder builder);
    }
    public class ConstruirSession : IConstruirSession
    {
        private readonly IConfiguration Configuracion;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public ConstruirSession(IHttpContextAccessor _HttpContextAccessor, IConfiguration _Configuracion)
        {
            Configuracion = _Configuracion;
            HttpContextAccessor = _HttpContextAccessor;
        }
        public DbContextOptions ObtenerConextOptions(DbContextOptionsBuilder builder)
        {
            var sesion = ObtenerSession();
            if (string.IsNullOrEmpty(sesion.CadenaConexion)) return builder.Options;


            builder.UseSqlServer(sesion.CadenaConexion, x =>
            {
                x.MigrationsHistoryTable("_MigrationHistory", "ADP_SRM");
                x.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
            });

            return builder.Options;
        }
        public SessionDTO ObtenerSession()
        {
            SessionDTO sesion = new SessionDTO();
            if (Configuracion["Enviroment"].ToString().Equals("Desarrollo"))
            {
                sesion.CadenaConexion = Configuracion.GetConnectionString("DevelopConnection");
                sesion.IdEmpresa = 1;
                sesion.IdSucursal = 392;
                sesion.IdUsuario = 50;

            }
            else
            {

                sesion.CadenaConexion = HttpContextAccessor.HttpContext.Items["_cadenaConexion"].ToString();
                sesion.IdEmpresa = Convert.ToInt32(HttpContextAccessor.HttpContext.Items["_idEmpresa"].ToString());
                sesion.IdSucursal = Convert.ToInt32(HttpContextAccessor.HttpContext.Items["_idSucursal"].ToString());
                sesion.IdUsuario = Convert.ToInt32(HttpContextAccessor.HttpContext.Items["_idUsuario"].ToString());


            }
            return sesion;
        }
    }
}
