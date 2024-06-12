
using Newtonsoft.Json;
using System.Text;


namespace HandleError
{
    public interface IHandleError
    {
        void GuardarError(Exception e, string method, string? dataRequest = "", object? data = null);
        void GuardarError(string e, string method, string dataRequest);
    }
    public class HandleExeption : IHandleError
    {

        public void GuardarError(Exception e, string method, string? dataRequest = "", object? data = null)
        {
            string _stackTrace = e.StackTrace ?? "";
            string base_error = e.ToString();
            string Controller = method;

            string dataSend = "";

            if (data != null)
            {
                dataSend = JsonConvert.SerializeObject(data);
            }
            else dataSend = dataRequest ?? "";

            string mensaje = CrearTextoError("SRM", _stackTrace, base_error, Controller, dataSend);

            string ruta_log = ValidarCarpetaErrorCLiente("SRM", mensaje);

            RegistrarError(ruta_log, mensaje);
        }

        public void GuardarError(string e, string method, string dataRequest)
        {

            string base_error = e.ToString();
            string Controller = method;
            string mensaje = CrearTextoError("PORTAL PROVEEDORES", string.Empty, base_error, Controller, dataRequest);

            string ruta_log = ValidarCarpetaErrorCLiente("PORTAL PROVEEDORES", mensaje);

            RegistrarError(ruta_log, mensaje);
        }

        void RegistrarError(string ruta, string texto)
        {

            using (StreamWriter file = new StreamWriter(ruta, true))
            {
                file.WriteLine(texto); //se agrega información al documento

                file.Close();
            }

        }

        string ValidarCarpetaErrorCLiente(string nombreEmpresa, string texto)
        {

            string ruta = AppContext.BaseDirectory.ToLower().Split(new string[] { "api" }, System.StringSplitOptions.RemoveEmptyEntries)[0] + @"api\Errores";

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);

            }

            ruta += @"\" + nombreEmpresa + ".txt";

            if (!File.Exists(ruta))
            {
                var archivo = File.Create(ruta);
                archivo.Close();
            }

            return ruta;
        }

        string CrearTextoError(string nombreCliente, string StackTrace, string exception, string _request, string dataRequest)
        {
            StringBuilder texto = new StringBuilder();

            texto.AppendLine("================ |INICIO DEL ERROR |====================================");
            texto.AppendLine(string.Format("CLIENTE: {0}", nombreCliente));
            texto.AppendLine(string.Format("------->Fecha y hora {0}", DateTime.Now.ToString("G")));
            texto.AppendLine("====================================================");
            texto.AppendLine("Eception: " + exception);
            texto.AppendLine("====================================================");
            texto.AppendLine("PayLoad: " + dataRequest);
            texto.AppendLine("====================================================");
            texto.AppendLine("REQUEST: " + _request);
            texto.AppendLine("====================================================");
            texto.AppendLine("StackTrace: " + StackTrace);
            texto.AppendLine("================ |FIN DEL ERROR |====================================");

            return texto.ToString();
        }
    }
}