using Model.DTO.ComprasD;
using Model.Entity.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventarios.DTO
{
    public class BodegasSucursalDTO
    {
        public BodegasSucursalDTO()
        {
            this.Bodega = new BodegasSucursal();
        }
        public BodegasSucursal Bodega { get; set; }
        public string FechaMaxMov { get; set; } = string.Empty;
    }
}
