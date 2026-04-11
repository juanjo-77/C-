using NeoGenesis.Infrastructure.Data;
using NeoGenesis.Shared;
using NeoGenesis.Shared.Entities;

namespace NeoGenesis.Modules.Create;

public class CreateHandler
{
    private readonly CreateValidator _validator;
    private readonly CreateService   _service;

    public CreateHandler(MySqlDbContext db)
    {
        _validator = new CreateValidator(db);
        _service   = new CreateService(db);
    }

    /// <summary>
    /// Orquesta el flujo completo de registro de un dinosaurio.
    /// </summary>
    public void Handle()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("     REGISTRO DE NUEVO DINOSAURIO    ");
        Console.WriteLine("====================================\n");

        // ── Campos obligatorios ──────────────────────────────────────
        Console.Write("Nombre asignado al dinosaurio : ");
        string dinoName = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Especie / Clasificación : ");
        string dinoEspecie = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Identificador único (sobre_nombre) : ");
        string sobreNombre = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Código de registro (nombre_sobreNombre) : ");
        string registerCode = Console.ReadLine()?.Trim() ?? string.Empty;

        // ── Campos opcionales ────────────────────────────────────────
        Console.WriteLine("\n── Datos opcionales (Enter para omitir) ──");

        Console.Write("Edad : ");
        string ageInput = Console.ReadLine()?.Trim() ?? string.Empty;
        int? age = int.TryParse(ageInput, out int parsedAge) ? parsedAge : null;

        Console.Write("Tipo (Carnívoro / Herbívoro) : ");
        string? type = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(type)) type = null;

        Console.Write("Zona del parque (Zone) : ");
        string? zone = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(zone)) zone = null;

        Console.Write("Sector del parque (Sector) : ");
        string? sector = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(sector)) sector = null;

        Console.Write("Dispositivo de rastreo (Phone) : ");
        string? phone = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(phone)) phone = null;

        Console.Write("Ubicación registrada (Address) : ");
        string? address = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(address)) address = null;

        // ── Validación ───────────────────────────────────────────────
        string? error = _validator.Validate(dinoName, dinoEspecie, sobreNombre, registerCode, age);

        if (error is not null)
        {
            Helpers.PrintError(error);
            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey();
            return;
        }

        // ── Construcción del objeto ──────────────────────────────────
        var dinosaur = new Dinosaur
        {
            DinoName = dinoName,
            DinoEspecie = dinoEspecie,
            SobreNombre = sobreNombre,
            RegisterCode = registerCode,
            Age = age,
            Type = type,
            Zone      = zone,
            Sector    = sector,
            Phone     = phone,
            Address   = address,
            CreatedAt = DateTime.UtcNow
        };

        // ── Persistencia ─────────────────────────────────────────────
        int newId = _service.Register(dinosaur);

        Helpers.PrintSuccess($"Dinosaurio registrado correctamente. ID asignado: {newId}");
        Console.WriteLine("\nPresione cualquier tecla para volver...");
        Console.ReadKey();
    }
}