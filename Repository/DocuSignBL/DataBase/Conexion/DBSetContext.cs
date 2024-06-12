using Microsoft.EntityFrameworkCore;
using Model.Entity.Addons;
using Model.Entity.ADP_API;
using Model.Entity.ADP_API_OBR;
using Model.Entity.ADP_PNL;
using Model.Entity.DBO;
using Model.Entity.Marco;

namespace Repository.DataBase.Conexion
{
    public partial class DB_ADPRO
    {
        #region DBO
        public DbSet<ADPConfig> adpconfig { get; set; }
        public DbSet<AdpContratos> contrato { get; set; }
        public DbSet<ADPTipoContratos> tipoContratos { get; set; }
        public DbSet<Terceros> tercero { get; set; }
        public DbSet<TercerosContactos> tercerosContactos { get; set; }
        public DbSet<TiposContacto> tiposContacto { get; set; }
        public DbSet<ADPObras> obra { get; set; }
        public DbSet<Compras> compras { get; set; }
        public DbSet<FormaPago> formaPago { get; set; }
        public DbSet<ComprasDet> comprasDet { get; set; }
        public DbSet<MovimientosInv> movimientosInv { get; set; }
        public DbSet<Monedas> monedas { get; set; }
        public DbSet<Sucursal> sucursal { get; set; }
        public DbSet<Producto> producto { get; set; }
        public DbSet<ADPEntradasAlmacen> adpEntradasAlmacen { get; set; }
        #endregion

        #region ADP_API_OBR

        public DbSet<ZonasObraAsignacion> zonasObraAsignacion { get; set; }

        #endregion


        #region ADP_API
        public DbSet<TokenDocusign> tokenDocusign { get; set; }
        public DbSet<MinutasFirmantes> MinutasFirmantes { get; set; }
        public DbSet<MinutasFirmantesZona> minutasfirmantesZona { get; set; }
        #endregion

        #region Addons
        public DbSet<AddonsListado> addonsListado { get; set; }
        public DbSet<AddonsConfig> addonsconfig { get; set; }
        #endregion

        #region Tablas del Marco
        public DbSet<Menus> menus { get; set; }
        #endregion


        #region Panel Informes

        public DbSet<PanelInforme> panelInforme { get; set; }
        public DbSet<ClaseInforme> claseInforme { get; set; }
        public DbSet<TipoInforme> tipoInforme { get; set; }

        #endregion
    }
}
