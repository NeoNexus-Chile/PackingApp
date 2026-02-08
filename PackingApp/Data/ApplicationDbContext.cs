using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PackingApp.Core;

namespace PackingApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<EstadoPedido> EstadosPedido { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Formato> Formatos { get; set; }
        public DbSet<Presentacion> Presentaciones { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }

}
