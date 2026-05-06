using TurnSystem.Web.Models;

namespace TurnSystem.Web.Services.Interfaces;

public interface ITurnoService
{
    Task<List<Turno>> ObtenerPorEstadoAsync(EstadoTurno estado);
    Task<Turno> GenerarTurnoAsync(int usuarioId, string? servicio = null, string? prioridad = null);
    Task<Turno?> ObtenerTurnoActivoAsync(int usuarioId);
    Task<Turno?> LlamarSiguienteAsync(string? asesorNombre = null, string? ventanilla = null);
    Task<bool> FinalizarAtencionAsync(int turnoId, string? comentario);
    Task<bool> MarcarNoPresentadoAsync(int turnoId);
    Task<bool> CancelarTurnoAsync(int turnoId);
    Task<bool> UsuarioTieneActivoAsync(int usuarioId);
    Task<(Usuario? usuario, Turno? turnoActivo)> ObtenerUsuarioYTurnoAsync(string documento);
}