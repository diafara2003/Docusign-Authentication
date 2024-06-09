using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Model.Entity.Marco
{
    [Table("menus", Schema = "dbo")]
    public class Menus
    {
        [Key]
        public int IdMenu { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public byte ActMenu { get; set; }
        public string Ruta { get; set; }
        public int Modulo { get; set; }
    }
}
