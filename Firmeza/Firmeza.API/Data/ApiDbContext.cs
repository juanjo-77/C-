using Firmeza.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.API.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithOne(d => d.Venta)
                .HasForeignKey(d => d.VentaId);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId);
        }
    }
}