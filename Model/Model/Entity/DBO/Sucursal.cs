using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("Sucursal")]
    public class Sucursal
    {
        [Key]
        public Int16 SucID { get; set; }
        public string SucDesc { get; set; }
    }
}
