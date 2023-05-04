using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("ADPObras")]
    public class ADPObras
    {
        [Key]
        public int ObrObra { get; set; }
        public string ObrNombre { get; set; }        
    }
}
