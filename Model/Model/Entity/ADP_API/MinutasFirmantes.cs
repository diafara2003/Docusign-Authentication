using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


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
