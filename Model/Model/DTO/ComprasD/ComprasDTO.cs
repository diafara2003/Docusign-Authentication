using Model.Entity.DBO;

namespace Model.DTO.ComprasD
{
    public class ComprasDTO
    {
        public Compras compra { get; set; }
        public Monedas moneda { get; set; }
        public Sucursal sucursal { get; set; }
        public Terceros terceros { get; set; }
        public EstadoOrdenCompraDTO EstadoOrden { get; set; }
    }
}
