using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("AdpContratos")]
    public class AdpContratos
    {
        [Key]
        public string ConID { get; set; }
        public string ConNumero { get; set; }
        public int ConContratista { get; set; }
        public int ConObra { get; set; }
    }
}
