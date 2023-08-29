
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("Terceros")]
   public  class Terceros
    {
        [Key]
        public int TerID { get; set; }
        public string TerNombre { get; set; }
        public string TerNit { get; set; }
        public string TerDireccion { get; set; }
        public string TerTelefono { get; set; }
        public int? TerPlazoPago { get; set; }
        public byte TerEstado { get; set; }
        public string TerTipo { get; set; }
        public string TerEmail { get; set; }
        public string TerEmailCto { get; set; }
    }
}
