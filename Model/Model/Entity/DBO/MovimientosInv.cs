
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity.DBO
{
    [Table("MovimientosInv")]
    public class MovimientosInv
    {
        [Key]
        public int MvIID { get; set; }
        public string MvITipo { get; set; }
        public long MvIDoc { get; set; }
        public decimal MvIVrUnit { get; set; }
        public decimal MvICant { get; set; }
        public decimal MvIVrUnitMM { get; set; }
        public decimal MvIVrTotal { get; set; }
        public decimal MvIVrTotalMM { get; set; }
        public decimal MvIBaseIvaDiff { get; set; }
        public decimal MvIBaseIvaDiff2 { get; set; }
        public int MvIBodega { get; set; }
        public int MvICompDetID { get; set; }
    }
}
