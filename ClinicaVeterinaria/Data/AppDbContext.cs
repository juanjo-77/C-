
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Data
{
    public class AppDbContext : DbContext
    {
        // DbSet representa la tabla Pacientes en MySQL
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=ClinicaVeterinaria;User=root;Password=Qwe.123*;",
                ServerVersion.AutoDetect("Server=localhost;Port=3306;Database=ClinicaVeterinaria;User=root;Password=Qwe.123*;")
            );
        }
    }
}