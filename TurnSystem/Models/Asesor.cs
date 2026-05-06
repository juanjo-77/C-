namespace TurnSystem.Web.Models;

public class Asesor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ventanilla { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}