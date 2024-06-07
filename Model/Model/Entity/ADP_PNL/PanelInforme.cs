using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model.Entity.ADP_PNL
{
    [Table("informe", Schema = "adp_pnl")]
    public class PanelInforme
    {
        [Key]
        public int InfId { get; set; }
        public int ClaseInf { get; set; }
        public string InfDesc { get; set; }
        public bool infMostrar { get; set; }
    }

    [Table("ClaseInforme", Schema = "adp_pnl")]
    public class ClaseInforme
    {
        [Key]
        public int PnlId { get; set; }
        public int TipInfo { get; set; }
        public string PnlDescripcion { get; set; }
        public bool PnlEstado { get; set; }
    }

    [Table("TipoInforme", Schema = "adp_pnl")]
    public class TipoInforme
    {
        [Key]
        public int TipoInfId { get; set; }        
        public string TipoInfDescripcion { get; set; }        
    }
}
