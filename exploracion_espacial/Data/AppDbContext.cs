using Microsoft.EntityFrameworkCore;
using exploracion_espacial.Models;

namespace exploracion_espacial.Data
{
    public class AppDbContext : DbContext
    {
        // Una DbSet por cada entidad = una tabla en la base de datos
        public DbSet<Astronauta> Astronautas { get; set; }
        public DbSet<Ingeniero> Ingenieros { get; set; }
        public DbSet<Nave> Naves { get; set; }
        public DbSet<Mision> Misiones { get; set; }
        public DbSet<RegistroExploracion> RegistrosExploracion { get; set; }

        // Le decimos a EF Core qué base de datos usar
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=astronova.db");
        }

        // Aquí configuramos las relaciones con Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación: Astronauta tiene muchas Misiones
            modelBuilder.Entity<Mision>()
                .HasOne(m => m.Astronauta)
                .WithMany(a => a.Misiones)
                .HasForeignKey(m => m.AstronautaId);

            // Relación: Nave tiene muchas Misiones
            modelBuilder.Entity<Mision>()
                .HasOne(m => m.Nave)
                .WithMany(n => n.Misiones)
                .HasForeignKey(m => m.NaveId);

            // Relación: Mision tiene muchos RegistrosExploracion
            modelBuilder.Entity<RegistroExploracion>()
                .HasOne(r => r.Mision)
                .WithMany(m => m.RegistroExploracion)
                .HasForeignKey(r => r.MisionId);
        }
    }
}