using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NeoGenesis.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
{
    public MySqlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
        optionsBuilder.UseMySql(
            "Server=204.168.211.73;Database=prueba_dino;User=root;Password=gWTeX0zTHgGQ6G1;",
            ServerVersion.AutoDetect("Server=204.168.211.73;Database=prueba_dino;User=root;Password=gWTeX0zTHgGQ6G1;")
        );

        return new MySqlDbContext(optionsBuilder.Options);
    }
}