using System.Text.RegularExpressions;
using NeoGenesis.Infrastructure.Data;

namespace NeoGenesis.Modules.Create;

public class CreateValidator
{
    private readonly MySqlDbContext _db;

    public CreateValidator(MySqlDbContext db) => _db = db;

    /// <summary>
    /// Valida todos los campos del registro. Retorna null si todo OK,
    /// o el mensaje de error si algo falla.
    /// </summary>
    public string? Validate(
        string dinoName, string dinoEspecie,
        string sobreNombre,  string registerCode,
        int?   age)
    {
        // 1. Campos obligatorios
        if (string.IsNullOrWhiteSpace(dinoName))
            return "El nombre es obligatorio.";

        if (string.IsNullOrWhiteSpace(dinoEspecie))
            return "La especie es obligatoria.";

        if (string.IsNullOrWhiteSpace(sobreNombre))
            return "El identificador es obligatorio.";

        if (string.IsNullOrWhiteSpace(registerCode))
            return "El código de registro es obligatorio.";


        // 3. Edad >= 0
        if (age.HasValue && age.Value < 0)
            return "La edad debe ser mayor o igual a 0.";

        // 4. Username único
        bool sobreNombreExists = _db.Dinosaurs.Any(d => d.SobreNombre == sobreNombre);
        if (sobreNombreExists)
            return $"El identificador '{sobreNombre}' ya está registrado en el sistema.";

        // 5. Email único
        bool registerCodeExists = _db.Dinosaurs.Any(d => d.RegisterCode == registerCode);
        if (registerCodeExists)
            return $"El código de registro '{registerCode}' ya existe en el sistema.";

        return null; // Sin errores
    }

}