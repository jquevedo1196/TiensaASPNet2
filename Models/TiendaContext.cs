using Microsoft.EntityFrameworkCore;

namespace tienda_web.Models
{
    public class TiendaContext: DbContext
    {
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<CatArticulo> CatArticulos { get; set; }
        public DbSet<CatTipoArt> CatTipoArts { get; set; }
        public DbSet<InvArticulo> InvArticulos { get; set; }

        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
        {

        }
    }
}