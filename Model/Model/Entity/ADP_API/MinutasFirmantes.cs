using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.ADP_API
{
    [Table("MinutasFirmantes", Schema = "adp_api")]
    public class MinutasFirmantes
    {
        [Key]
        public int MFId { get; set; }
        public string MFDescripcion { get; set; }
        public string MFNombre { get; set; }
        public string MFCorreo { get; set; }
        public bool MFIsEditable{ get; set; }
    }
}
