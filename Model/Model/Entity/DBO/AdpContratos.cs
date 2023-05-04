using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("AdpContratos")]
    public class AdpContratos
    {
        [Key]
        public int ConID { get; set; }
        public int ConNumero { get; set; }
        public int ConContratista { get; set; }
        public int ConObra { get; set; }
        public string ConTipoContrato { get; set; }
    }
}
