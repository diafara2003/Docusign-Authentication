using Model.DTO.Docusign;
using Model.Entity.DBO;
using System.Collections.Generic;

namespace Model.DTO.ComprasD
{
    public class ComprasDTO
    {
        public ComprasDTO()
        {
            this.compra = new Compras();
            this.moneda = new Monedas();
            this.sucursal = new Sucursal();
            this.terceros = new Terceros();
            this.EstadoOrden = new EstadoOrdenCompraDTO();
        }
        public Compras compra { get; set; }
        public Monedas moneda { get; set; }
        public Sucursal sucursal { get; set; }
        public Terceros terceros { get; set; }
        public EstadoOrdenCompraDTO EstadoOrden { get; set; }
    }
}
