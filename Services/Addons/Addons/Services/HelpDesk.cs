
using Addons.DTO;
using HandleError;
using Microsoft.AspNetCore.Http;
using Model.DTO;
using Newtonsoft.Json;
using Repository.DataBase.Conexion;
using Repository.DataBase.Model;
using System.Text;

namespace Addons.Services
{
    public interface IHelpDesk
    {
        void CrearSoporte(string msjError, string asunto = "Soporte Automático");
    }
    public class HelpDesk : IHelpDesk
    {
        private DB_ADPRO _contexto;
        private IHandleError _error;
        private IConstruirSession session;
        private static int EquipoTrabajo = 55555;
        private static string Responsable_Id = "469";
        private static string Modulo = "265";

        public HelpDesk(DB_ADPRO contexto, HandleExeption error, IConstruirSession _session)
        {
            _contexto = contexto;
            _error = error;
            session = _session;
        }

        public void CrearSoporte(string msjError, string asunto = "Soporte Automático")
        {
            var data = session.ObtenerSession();


            string idEmpresa = data.IdEmpresa.ToString();
            string IdSucursal = data.IdSucursal.ToString();
            string IdProyecto = data.IdProyecto.ToString();
            int IdUsuario = data.IdUsuario;
            int Equipotrabajo_Id = EquipoTrabajo;



            List<DateTime> festivos = FestivosColombia.DiasFestivos(DateTime.Now.Year);
            int dias = 1;
            DateTime fechaHoy = DateTime.Now.AddDays(+Convert.ToInt32(dias));
            bool fechaValida = FechaValidaHD(festivos, fechaHoy);
            while (fechaValida == false)
            {
                fechaHoy = fechaHoy.AddDays(1);

                fechaValida = FechaValidaHD(festivos, fechaHoy);
            }
            if (DateTime.Now.Hour >= 13)
            {
                fechaValida = false;
                while (fechaValida == false)
                {
                    fechaHoy = fechaHoy.AddDays(1);
                    fechaValida = FechaValidaHD(festivos, fechaHoy);
                }
            }


            var dt = _contexto.ExecuteStoreQuery(new Repository.DataBase.Model.ProcedureDTO()
            {
                commandText = "[ADP_API].[VariablesConfiguracionSoporte]",
                parametros = new Dictionary<string, object>()
            {
                {"@idUsuario",IdUsuario },{"@IdSucursal",IdSucursal },{"@IdEmpresa",idEmpresa },
                {"@IdProyecto",IdProyecto }
            }
            });


            if (dt.Rows.Count > 0)
            {
                string EMPRESA_ID = dt.Rows[0]["EMPRESA_ID"].ToString();
                string BASE_DATOS_ID = dt.Rows[0]["BASE_DATOS_ID"].ToString();
                string NombreUsuario = dt.Rows[0]["NombreUsuario"].ToString();
                string NombreSucursal = dt.Rows[0]["NombreSucursal"].ToString();
                string NombreEmpresa = dt.Rows[0]["NombreEmpresa"].ToString();

                idEmpresa = dt.Rows[0]["IdEmpresaOrign"].ToString();
                //IdSucursal = dt.Rows[0]["IdSucursalOrign"].ToString();
                //IdProyecto = dt.Rows[0]["IdProyectoOrign"].ToString();


                HelpDesckObject helpDesckObjectA = new HelpDesckObject();
                Historial historial = new Historial
                {
                    Detalles = "[{\"propiedad\":\"Origen\",\"valorAnterior\":null,\"valorNuevo\":\"Soporte automático creado desde el Administrador de ADPRO\"}]",
                    Usuario = "MARCO"
                };

                List<Historial> list = new List<Historial> { historial };
                helpDesckObjectA.Historial = list;

                helpDesckObjectA.CantidadReplicas = "0";
                helpDesckObjectA.SoporteOriginal_Id = null;
                helpDesckObjectA.Asunto = asunto;
                helpDesckObjectA.Mensaje = "<b>Soporte Automático</b><br/>" + msjError;
                helpDesckObjectA.Tipo = "ADDON";
                helpDesckObjectA.IdBaseDatos = Convert.ToInt32(BASE_DATOS_ID);
                helpDesckObjectA.Empresa_Id = Convert.ToInt32(EMPRESA_ID);



                helpDesckObjectA.ValoresSesion = "{\"nombreUsuario\":\"" + NombreUsuario + "\",\"idEmpresa\":\"" + idEmpresa + "\",\"idBaseDatos\":" + BASE_DATOS_ID + ",\"idEmpresaCliente\":" + EMPRESA_ID + ",\"NomEmpresa\":\"" + NombreEmpresa + "\",\"NombreEmpresa\":\"" + NombreEmpresa + "\",\"NombreSucursal\":\"" + NombreSucursal + "\"}";
                helpDesckObjectA.Valoresnavegador = "";
                helpDesckObjectA.Ultimanavegacion = "";
                helpDesckObjectA.Menuseleccionado = "";
                helpDesckObjectA.Interesados = "[{\"nombre\":\"" + NombreUsuario + "\",\"cargo\":\"\",\"email\":[\"carlos@sinco.co\"],\"telefonos\":[]}]";
                helpDesckObjectA.Prioridad = "Baja";
                System.DateTime moment = fechaHoy;
                helpDesckObjectA.FechaVencimiento = /*"2020-04-22T08:52:44.000Z";*/moment.Year.ToString() + "-" + (((moment.Month <= 9) ? "0" : "") + moment.Month.ToString()) + "-" + (((moment.Day <= 9) ? "0" : "") + moment.Day.ToString()) + "T18:00:00.000Z";

                #region Adjuntos
                //if (AdjuntosListA == null)
                //{
                //    List<Adjunto> AdjuntosList = new List<Adjunto>();

                //    string[] fechaI = DateTime.Now.ToString("yyyy-MM-dd HH:mm").Replace("-", "").Split(' ');
                //    string fechaYMD = fechaI[0];
                //    string fechaHr = fechaI[1].Split(':')[0];
                //    int fechamm = Convert.ToInt32(fechaI[1].Split(':')[1]);

                //    string[] separatorsAB = { "CATALOG=" };
                //    string[] urlSplitABC = cadenaConexion.ToUpper().Split(separatorsAB, StringSplitOptions.None);
                //    string basededatosC = "MG_" + urlSplitABC[1].Split(';')[0] + "_";


                //    Adjunto adjuntosC = new Adjunto();
                //    try
                //    {
                //        string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/UploadFiles/TempMigraExcel/").Replace("SDL", "API"));
                //        foreach (string filename in files)
                //        {
                //            if (filename.Contains(fechaYMD + "_" + fechaHr) && filename.Contains(basededatosC))
                //            {
                //                for (int i = fechamm - 5; i <= fechamm; i++)
                //                {
                //                    string countV = i < 10 ? "0" + i.ToString() : i.ToString();
                //                    if (filename.Contains(fechaYMD + "_" + fechaHr + countV))
                //                    {
                //                        FileInfo fi = new FileInfo(filename);
                //                        Byte[] bytes = File.ReadAllBytes(filename);
                //                        String file = Convert.ToBase64String(bytes);
                //                        adjuntosC.Archivo = file;
                //                        adjuntosC.Nombre = filename.Replace(@"\", "@").Split('@')[filename.Replace(@"\", "@").Split('@').Length - 1];
                //                        helpDesckObjectA.Mensaje = "<b>Soporte Automático</b><br/>Revisar Adjunto anexo en este Soporte<br/>" + (msjErrorMG != "" ? msjErrorMG : msjError);
                //                        fi.Delete();
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    catch
                //    {

                //    }
                //    if (adjuntosC.Nombre != null)
                //    {
                //        AdjuntosList.Add(adjuntosC);
                //    }
                //    ////var base64Adj = adjuntos.adjunto;
                //    Adjunto Adjuntob = new Adjunto();
                //    if (adjuntos != null && adjuntos.Archivo != null)
                //    {
                //        Adjuntob.Archivo = adjuntos.Archivo.Replace("data:image/png;base64,", "");

                //        Adjuntob.Nombre = string.IsNullOrEmpty(adjuntos.Nombre) ? "SCREENSHOT_SOPORTE.png" : adjuntos.Nombre;
                //        Adjuntob.Usuario = "Admin Sinco Comunicaciones";
                //        AdjuntosList.Add(Adjuntob);
                //    }
                //    helpDesckObjectA.Adjuntos = AdjuntosList;
                //}
                //else
                //{
                //    helpDesckObjectA.Adjuntos = AdjuntosListA;
                //}
                #endregion

                helpDesckObjectA.AccesoProduccion = true;
                helpDesckObjectA.Estado_Id = 1;
                helpDesckObjectA.EnviadoASincosoft = 1;
                helpDesckObjectA.Responsable_Id = Responsable_Id;
                helpDesckObjectA.Equipotrabajo_Id = Equipotrabajo_Id.ToString();
                helpDesckObjectA.Id_Torre_V1 = null;
                string DATA = System.Text.Json.JsonSerializer.Serialize(data);

                string url = "http://sincoerp.com/sincosoporte/urlsoporte.js";
                try
                {
                    HttpClient httpClientA = new HttpClient();
                    HttpResponseMessage resultUrl = httpClientA.GetAsync(url).Result;

                    string resUrlPrecharged = "";

                    if (resultUrl.ToString().ToUpper().IndexOf("NOT FOUND") >= 0)
                        resUrlPrecharged = "'https://core.sincoerp.com/sincosoporte'";


                    string contents = resultUrl.Content.ReadAsStringAsync().Result;
                    StringContent content = new StringContent(DATA.ToString(), Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();
                    try
                    {

                        string urlCargada = contents.Split('=')[1].Trim().Replace("'", "").Replace(";", "");
                        if (resUrlPrecharged != "")
                        {
                            resUrlPrecharged = resUrlPrecharged.Replace("'", "").Replace(";", "");
                            urlCargada = resUrlPrecharged;
                        }
                        //if (tipoBD == "3" || HttpContext.Current.Request.Url.Host.ToString().ToUpper().IndexOf("DESARROLLO.SINCOERP") >= 0 || HttpContext.Current.Request.Url.Host.ToString().ToUpper().IndexOf("LOCALHOST") >= 0)
                        //{
                        //    urlCargada = urlCargada.Replace("sincosoporte", "sincosoporte_pruebas");
                        //}
                        System.Threading.Tasks.Task<HttpResponseMessage> result = httpClient.PostAsync(urlCargada + "/Api/Mensajes", content);


                    }
                    catch (Exception EX)
                    {
                        _error.GuardarError(EX, "CrearSoporte");

                    }
                }
                catch (Exception EX)
                {
                    _error.GuardarError(EX, "CrearSoporte");

                }
            }
        }
        bool FechaValidaHD(List<DateTime> festivos, DateTime fechaHoy)
        {
            int i;
            bool fechaValida = true;
            for (i = 0; i < festivos.Count; i++)
            {
                if (fechaHoy.ToString().Split(' ')[0] == festivos[i].ToString().Split(' ')[0])
                {
                    fechaValida = false;
                    i = festivos.Count + 1;
                }
            }
            return fechaValida;
        }
    }
}
