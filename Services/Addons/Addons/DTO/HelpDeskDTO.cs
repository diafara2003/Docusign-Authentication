
namespace Addons.DTO
{
    public class HelpDesckObject
    {
        public IList<Historial> Historial { get; set; }
        public string CantidadReplicas { get; set; }
        public string SoporteOriginal_Id { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public int IdBaseDatos { get; set; }
        public int Empresa_Id { get; set; }
        public string ValoresSesion { get; set; }
        public string Valoresnavegador { get; set; }
        public string Ultimanavegacion { get; set; }
        public string Menuseleccionado { get; set; }
        public string Interesados { get; set; }

        public string Prioridad { get; set; }
        public string FechaVencimiento { get; set; }
        public IList<Adjunto> Adjuntos { get; set; }
        public bool AccesoProduccion { get; set; }
        public int Estado_Id { get; set; }
        public int EnviadoASincosoft { get; set; }
        public string Responsable_Id { get; set; }
        public string Equipotrabajo_Id { get; set; }
        public string Id_Torre_V1 { get; set; }
    }

    public class Historial
    {
        public string Detalles { get; set; }
        public string Usuario { get; set; }
    }

    public class Adjunto
    {
        public string Nombre { get; set; }
        public string Archivo { get; set; }

        public Array File { get; set; }
        public string Usuario { get; set; }
    }
}
