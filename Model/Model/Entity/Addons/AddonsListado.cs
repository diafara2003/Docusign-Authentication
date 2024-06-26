using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Model.Entity.Addons
{
    [Table("Listado", Schema = "addons")]
    public class AddonsListado
    {
        [Key]
        public int IdListado { get; set; }
        public int Modulo { get; set; }
        public int AddonNumero { get; set; }
        public string AddonNombre { get; set; }
        public short HorasInstalacion { get; set; }
        public decimal HorasBdAdicional { get; set; }
        public string    SubModulo { get; set; }
        public string Obs { get; set; }
        public decimal? costo { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaPublicacion { get; set; }
        public string RutaPDF { get; set; }
        public string Requisitos { get; set; }
        public bool Obsoleto { get; set; }
        public string Reemplazo { get; set; }
        public bool? Estandar { get; set; }
        public bool Principales { get; set; }
        public string ModuloRequisito { get; set; }
        public bool Instalar { get; set; }
        public bool Ver { get; set; }
        
    }
}
