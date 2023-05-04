using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("ADPTipoContratos")]
    public class ADPTipoContratos
    {
        [Key]
        public string TcnCodigo { get; set; }
        public string TcnDesc { get; set; }        
    }
}
