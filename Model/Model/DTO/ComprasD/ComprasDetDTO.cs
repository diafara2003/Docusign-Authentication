using Model.Entity.DBO;

namespace Model.DTO.ComprasD
{
    public class ComprasDetDTO
    {
        public ComprasDet comprasDet { get; set; }
        public Producto producto { get; set; }
        public bool BIDIFF { get; set; }

    }
}
