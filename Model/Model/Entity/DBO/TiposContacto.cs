
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("TiposContacto")]
   public class TiposContacto
    {
        [Key]
        public int TipoContId { get; set; }
        public string TipoContDesc { get; set; }        
    }
}
