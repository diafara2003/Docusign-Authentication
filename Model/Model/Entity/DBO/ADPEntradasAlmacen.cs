using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.DBO
{
    [Table("ADPEntradasAlmacen")]
    public class ADPEntradasAlmacen
    {
        [Key]
        public int EnAID { get; set; }
        public int EnANo { get; set; }
        public int EnAOC { get; set; }
        public int EnASuc { get; set; }
        public int EnAObra { get; set; }
        public int EnAUsu { get; set; }
        public string EnAFac { get; set; }
        public DateTime EnAFecha { get; set; }
        public DateTime EnAFechaFac { get; set; }
        public DateTime EnAFechaPago { get; set; }
        public DateTime EnAFechaReciboNo { get; set; }
        public Int16 EnAMoneda { get; set; }
        public string EnAObs { get; set; }

    }
}
