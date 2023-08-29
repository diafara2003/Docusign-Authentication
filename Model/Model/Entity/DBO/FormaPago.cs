using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("FormaPago")]
    public class FormaPago
    {
        [Key]
        public int FrPID { get; set; }
        public int FrPDias { get; set; }

    }
}