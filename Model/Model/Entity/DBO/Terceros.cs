
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
        public string TerEmail { get; set; }
    }
}
