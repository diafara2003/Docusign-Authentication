
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.Entity.DBO
{
    [Table("ADPConfig")]
    public class ADPConfig
    {
        [Key]
        public string CnfCodigo { get; set; }
        public string CnfValor { get; set; }
        public int CnfGrupos { get; set; }
    }
}
