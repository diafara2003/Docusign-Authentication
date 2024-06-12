

namespace Addons.DTO
{
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
