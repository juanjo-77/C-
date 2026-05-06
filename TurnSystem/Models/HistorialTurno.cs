namespace TurnSystem.Web.Models;

public class HistorialTurno
{
    public int Id { get; set; }

    public EstadoTurno EstadoAnterior { get; set; }
    public EstadoTurno EstadoNuevo { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public string? Nota { get; set; }

    // FK Turno
    public int TurnoId { get; set; }
    public Turno Turno { get; set; } = null!;
}