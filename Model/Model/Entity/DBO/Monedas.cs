using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("Monedas")]
    public class Monedas
    {
        [Key]
        public Int16 MonID { get; set; }
        public string MonDesc { get; set; }
        public string MonAbrev { get; set; }
        public string MonSimbolo { get; set; }
        public bool MonFuncional { get; set; }
    }
}
