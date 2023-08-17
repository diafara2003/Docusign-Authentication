using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using SincoSoft.Context.Core;
using System;


namespace Repository.DataBase.Model
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
                //x.MigrationsHistoryTable("_MigrationHistory", "ADP_SRM");
                //x.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
            });

            return builder.Options;
        }
        public SessionDTO ObtenerSession()
        {
            SessionDTO sesion = new SessionDTO();
            CurrentContext contextoActual = new CurrentContext(HttpContextAccessor);


            sesion.IdEmpresa = contextoActual.IdEmpresa;
            sesion.IdSucursal = contextoActual.IdSucursal;
            sesion.IdUsuario = contextoActual.IdUsuario;
            sesion.IdProyecto = contextoActual.IdProyecto;



            return sesion;
        }
    }
}
