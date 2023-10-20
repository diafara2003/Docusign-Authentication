
namespace Model.DTO
{
    public class ResponseAPIDTO
    {
        public bool success { get; set; }
    }



    public class ResponseV3DTO
    {
        public ResponseV3DTO()
        {
            this.codigo = 1;
            this.mensaje = "";
            this.id = "";
            this.TipoM = 0;
            this.Tipomigracion = 0;
        }
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public decimal diferencia { get; set; }
        public string id { get; set; }
        public string OtroValor { get; set; } = string.Empty;
        public decimal valor { get; set; }
        public int Tipomigracion { get; set; }
        public int TipoM { get; set; }
    }
}
