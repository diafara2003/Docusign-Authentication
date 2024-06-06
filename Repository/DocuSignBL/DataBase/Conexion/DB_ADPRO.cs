using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Entity.ADP_API;
using Model.Entity.ADP_API_OBR;
using Model.Entity.DBO;
using Repository.DataBase.Model;
using SincoSoft.Context.Core;

namespace Docusign.Repository.DataBase.Conexion
{
    public class DB_ADPRO : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor { get; }

        #region DBO
        public DbSet<ADPConfig> adpconfig { get; set; }
        public DbSet<AdpContratos> contrato { get; set; }
        public DbSet<ADPTipoContratos> tipoContratos { get; set; }
        public DbSet<Terceros> tercero { get; set; }
        public DbSet<TercerosContactos> tercerosContactos { get; set; }
        public DbSet<TiposContacto> tiposContacto { get; set; }
        public DbSet<ADPObras> obra { get; set; }
        public DbSet<Compras> compras { get; set; }
        public DbSet<FormaPago> formaPago { get; set; }
        public DbSet<ComprasDet> comprasDet { get; set; }
        public DbSet<MovimientosInv> movimientosInv { get; set; }
        public DbSet<Monedas> monedas { get; set; }
        public DbSet<Sucursal> sucursal { get; set; }
        public DbSet<Producto> producto { get; set; }
        public DbSet<ADPEntradasAlmacen> adpEntradasAlmacen { get; set; }
        #endregion


        #region ADP_API_OBR

        public DbSet<ZonasObraAsignacion> zonasObraAsignacion { get; set; }

        #endregion


        #region ADP_API
        public DbSet<TokenDocusign> tokenDocusign { get; set; }
        public DbSet<MinutasFirmantes> MinutasFirmantes { get; set; }
        public DbSet<MinutasFirmantesZona> minutasfirmantesZona { get; set; }
        #endregion

        public DB_ADPRO(IConstruirSession construirSession, IHttpContextAccessor httpContextAccessor) : base(construirSession.ObtenerConextOptions(new DbContextOptionsBuilder<DB_ADPRO>()))
        {
            this._httpContextAccessor = httpContextAccessor;
            //  var cliente = new SqlConnectionStringBuilder(construirSession.ObtenerSession().CadenaConexion);
            // if (!ConstantesEntornoEstaticas.MigracionesAplicadasBD.Any(bd => bd.Equals(cliente.InitialCatalog)))
            //{
            //if (Database.CanConnect())
            //{
            //    if (Database.GetPendingMigrations().Any()) Database.Migrate();
            //}
            //else
            //    throw new Exception("No se encontro la BD");

            //  ConstantesEntornoEstaticas.MigracionesAplicadasBD.Add(cliente.InitialCatalog);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            CurrentContext contextoActual = new CurrentContext(this._httpContextAccessor);
            // connect to sql server with connection string from app settings
            //options.UseSqlServer(contextoActual.CadenaConexion);
            options.UseSqlServer($"{contextoActual.CadenaConexion};TrustServerCertificate=True");
        }



    }

}
