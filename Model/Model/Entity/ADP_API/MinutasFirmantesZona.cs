
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Model.Entity.ADP_API
{
    [Table("MinutasFirmantesZona", Schema = "adp_api")]
    public class MinutasFirmantesZona
    {
        [Key]
        public int Id { get; set; }
        public int IdFirmante { get; set; }
        public int IdZona { get; set; }
        public int IdObra { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}
