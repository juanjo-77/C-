namespace NeoGenesis.Shared.Entities;

public class Dinosaur
{
    public int Id { get; set; }

    // Obligatorios
    public string DinoName { get; set; } = string.Empty;   // Nombre asignado
    public string DinoEspecie  { get; set; } = string.Empty;   // Especie / clasificación
    public string SobreNombre  { get; set; } = string.Empty;   // Identificador único
    public string RegisterCode { get; set; } = string.Empty;   // Código de registro

    // Opcionales
    public string? Phone    { get; set; }   // Dispositivo de rastreo
    public string? Address  { get; set; }   // Ubicación registrada
    public string? Zone     { get; set; }   // Zona del parque  (ciudad)
    public string? Sector   { get; set; }   // Sector del parque (país)
    public string? Type     { get; set; }   // Carnívoro / Herbívoro
    public int?    Age      { get; set; }
    public string? Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}