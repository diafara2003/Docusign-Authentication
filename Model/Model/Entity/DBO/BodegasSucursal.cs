using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("BodegasSucursal")]
    public class BodegasSucursal
    {
        [Key]
        public int BoSAutoNum { get; set; }
        public int BoSId { get; set; }
        public short BoSucursal { get; set; }
        public string BoSDescripcion { get; set; }
    }
}
