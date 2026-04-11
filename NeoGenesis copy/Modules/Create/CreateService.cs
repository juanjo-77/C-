using NeoGenesis.Infrastructure.Data;
using NeoGenesis.Shared.Entities;

namespace NeoGenesis.Modules.Create;

public class CreateService
{
    private readonly MySqlDbContext _db;

    public CreateService(MySqlDbContext db) => _db = db;

    /// <summary>
    /// Persiste el dinosaurio en la base de datos y retorna el Id generado.
    /// </summary>
    public int Register(Dinosaur dinosaur)
    {
        _db.Dinosaurs.Add(dinosaur);
        _db.SaveChanges();
        return dinosaur.Id;
    }
}