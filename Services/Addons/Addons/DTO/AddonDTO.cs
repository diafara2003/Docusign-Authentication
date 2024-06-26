

namespace Addons.DTO
{

    public class MarcarObsoletoAddonDTO
    {
        public int id { get; set; }
        public string reemplazo { get; set; }

    }
    public class PublicarAddonDTO
    {
        public int id { get; set; }
        public int numero { get; set; }
        public bool instalable { get; set; }
    }
    public class ResponseActivacionDTO
    {
        public string NombreDB { get; set; }
        public string Resultado { get; set; }
        public string Proceso { get; set; }
        public int Externo { get; set; }
    }

    public class RequestHDSolicitudDTO
    {
        public int addonno { get; set; }
        public string obs { get; set; }
        public List<string> entornos { get; set; }

    }
}
