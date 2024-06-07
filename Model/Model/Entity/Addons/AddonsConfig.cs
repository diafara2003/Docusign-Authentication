
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity.Addons
{
    [Table("Config", Schema = "addons")]
    public class AddonsConfig
    {
        [Key]
        public int IdConfig { get; set; }
        public int IdListado { get; set; }
        public string Tabla { get; set; }
        public string ColCodigoNombre { get; set; }
        public string ColCodigoValor { get; set; }
        public string ColValorNombre { get; set; }
        public string ColValorValor { get; set; }
        public string Obs { get; set; }
        public bool AnclarVarlor { get; set; }

    }
}
