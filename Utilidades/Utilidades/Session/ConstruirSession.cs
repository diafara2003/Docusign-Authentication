using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;


namespace Utilidades.Session
{

    public interface IConstruirSession
    {
        Session ObtenerSession();
        DbContextOptions ObtenerConextOptions(DbContextOptionsBuilder builder);
    }
    public class ConstruirSession : IConstruirSession
    {
        private readonly IConfiguration Configuracion;
        private readonly IHttpContextAccessor HttpContextAccessor;

        //public ConstruirSession(IConfiguration configuracion, IHttpContextAccessor _HttpContextAccessor)
        //{
        //    Configuracion = configuracion;
        //    HttpContextAccessor = _HttpContextAccessor;
        //}
        public DbContextOptions ObtenerConextOptions(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(ObtenerSession().CadenaConexion, x =>
            {
                x.MigrationsHistoryTable("_MigrationHistory", "ADP_SRM");
                x.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
            }); return builder.Options;
        }
        public Session ObtenerSession()
        {
            Session sesion = new Session(); if (Configuracion["Enviroment"].ToString().Equals("Desarrollo"))
            {
                sesion.CadenaConexion = Configuracion.GetConnectionString("DevelopConnection");
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
