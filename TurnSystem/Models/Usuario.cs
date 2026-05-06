using System.ComponentModel.DataAnnotations;

namespace TurnSystem.Web.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required] [StringLength(20)] public string Documento { get; set; } = string.Empty;
    
    [Required] [StringLength(100)] public string Nombre { get; set; } = string.Empty;
    
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [StringLength(256)] public string? PasswordHash { get; set; }
    
    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}