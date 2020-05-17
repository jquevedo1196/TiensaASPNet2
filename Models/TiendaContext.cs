using Microsoft.EntityFrameworkCore;

namespace tienda_web.Models
{
    public class TiendaContext: DbContext
    {
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<CatArticulo> CatArticulos { get; set; }
        public DbSet<CatTipoArt> CatTipoArts { get; set; }
        public DbSet<InvArticulo> InvArticulos { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        //public DbSet<AspNetUserRol> AspNetUserRoles { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
        {

        }
    }
}