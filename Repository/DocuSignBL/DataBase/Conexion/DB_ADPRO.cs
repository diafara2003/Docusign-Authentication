using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Docusign.Utilidades.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public DataTable ExecuteStoreQuery(ProcedureDTO obj)
        {
            string _cnn = string.Empty;

            //TOKEN
            CurrentContext contextoActual = new CurrentContext(_httpContextAccessor);
            _cnn = contextoActual.CadenaConexion;

            DataTable ds = new DataTable();
            using (SqlConnection context = new SqlConnection(_cnn))

            {
                SqlCommand cmd = new SqlCommand(obj.commandText.Trim(), context);
                if (obj.parametros != null)
                {
                    foreach (var item in obj.parametros)
                    {
                        SqlParameter objsp = new SqlParameter();
                        objsp.ParameterName = item.Key;
                        objsp.Value = item.Value;

                        cmd.Parameters.Add(objsp);
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 200;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    da.Fill(ds);

                }
                catch (Exception E)
                {
                    if (context.State != ConnectionState.Closed)
                    {
                        context.Close();
                    }
                    throw new ArgumentException(E.Message + E.Source + E.StackTrace + E.TargetSite + E.HelpLink + E.HelpLink);
                }
                context.Close();
            }
            return ds;
        }

    }

}
