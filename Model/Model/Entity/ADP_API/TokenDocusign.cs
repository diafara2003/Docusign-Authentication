using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity.ADP_API
{
    [Table("TokenDocusign",Schema = "adp_api")]
    public class TokenDocusign
    {
        [Key]
        public int TokenDocuId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public bool EnProceso { get; set; }
    }
}
