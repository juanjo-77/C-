using System.ComponentModel.DataAnnotations;

namespace TurnSystem.Web.Models;

public class Turno
{
    public int Id { get; set; }

    [Required]
    public string Ticket { get; set; } = string.Empty;

    public EstadoTurno Estado { get; set; } = EstadoTurno.Pendiente;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaAtencion { get; set; }
    public DateTime? FechaFin { get; set; }

    public string? Servicio { get; set; }
    public string Prioridad { get; set; } = "Normal";
    public string? Comentario { get; set; }

    // FK Usuario
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    // FK Asesor (nullable, se asigna cuando es llamado)
    public int? AsesorId { get; set; }
    public Asesor? Asesor { get; set; }

    // Datos del asesor al llamar el turno (string simple)
    public string? AsesorNombre { get; set; }
    public string? Ventanilla { get; set; }

    // Navegación
    public ICollection<HistorialTurno> Historial { get; set; } = new List<HistorialTurno>();
}