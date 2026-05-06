using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Models;

namespace TurnSystem.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<Asesor> Asesores { get; set; }
    public DbSet<HistorialTurno> HistorialTurnos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Documento único por usuario
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Documento)
            .IsUnique();

        // Turno → Usuario (un usuario tiene muchos turnos)
        modelBuilder.Entity<Turno>()
            .HasOne(t => t.Usuario)
            .WithMany(u => u.Turnos)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Turno → Asesor (nullable)
        modelBuilder.Entity<Turno>()
            .HasOne(t => t.Asesor)
            .WithMany(a => a.Turnos)
            .HasForeignKey(t => t.AsesorId)
            .OnDelete(DeleteBehavior.SetNull);

        // HistorialTurno → Turno
        modelBuilder.Entity<HistorialTurno>()
            .HasOne(h => h.Turno)
            .WithMany(t => t.Historial)
            .HasForeignKey(h => h.TurnoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}