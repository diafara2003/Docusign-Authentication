
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.DataBase.Model;
using SincoSoft.Context.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Repository.DataBase.Conexion
{
    public partial class DB_ADPRO : DbContext
    {
        private IHttpContextAccessor _httpContextAccessor { get; }

        public DB_ADPRO(IConstruirSession construirSession, IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;

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
