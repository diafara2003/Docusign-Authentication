
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("TercerosContactos")]
   public  class TercerosContactos
    {
        [Key]
        public int TCId { get; set; }
        public int TCTercero { get; set; }
        public int TCTipo { get; set; }
        public string TCMail { get; set; }
    }
}
