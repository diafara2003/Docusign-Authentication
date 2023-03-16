using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity.ADP_API_OBR
{
    [Table("ZonasObraAsignacion", Schema = "ADP_API_OBR")]
    public class ZonasObraAsignacion
    {
        
        public int ZOAIdZona { get; set; }
        public int ZOAIdObra { get; set; }
        [Key]
        public int ZOAid { get; set; }
    }
}
