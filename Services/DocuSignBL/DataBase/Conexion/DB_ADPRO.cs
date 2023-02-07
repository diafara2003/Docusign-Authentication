using System;
using System.Data;
using System.Data.SqlClient;
using DocuSignBL.DataBase.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model.Entity.ADP_API;
using Model.Entity.DBO;
using SincoSoft.Context.Core;

namespace DocuSignBL.DataBase.Conexion
{
    public class DB_ADPRO : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor { get; }

        public DbSet<TokenDocusign> tokenDocusign { get; set; }
        public DbSet<ADPConfig> adpconfig { get; set; }

        public DB_ADPRO(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
          
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            CurrentContext contextoActual = new CurrentContext(_httpContextAccessor);
            // connect to sql server with connection string from app settings
            options.UseSqlServer(contextoActual.CadenaConexion);
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


    public class Inicializer
    {

        public void UpgradeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DB_ADPRO>();
                if (context != null && context.Database != null)
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
