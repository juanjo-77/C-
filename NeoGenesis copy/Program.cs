using Microsoft.EntityFrameworkCore;
using NeoGenesis.Infrastructure.Data;
using NeoGenesis.Modules.Create;

// ── Conexión MySQL ──────────────────────────────────────────────────────
var connectionString =
    "Server=204.168.211.73;Database=prueba_dino;User=root;Password=gWTeX0zTHgGQ6G1;";

var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();
optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

using var db = new MySqlDbContext(optionsBuilder.Options);
db.Database.EnsureCreated();

// ── Menú principal ───────────────────────────────────────────────────────────
bool running = true;
while (running)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║        NEOGENESIS PARK — MENÚ        ║");
    Console.WriteLine("╚══════════════════════════════════════╝");
    Console.WriteLine("  1. Registrar nuevo dinosaurio");
    Console.WriteLine("  0. Salir");
    Console.Write("\nOpción: ");

    string option = Console.ReadLine() ?? "";

    switch (option)
    {
        case "1":
            var handler = new CreateHandler(db);
            handler.Handle();
            break;
        case "0":
            running = false;
            Console.WriteLine("\n¡Hasta pronto, investigador!");
            break;
        default:
            Console.WriteLine("Opción no válida. Presione una tecla...");
            Console.ReadKey();
            break;
    }
}