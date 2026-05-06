using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Data;
using TurnSystem.Web.Models;
using TurnSystem.Web.Services;

namespace TurnSystem.Web.Hubs;

public class TurnosHub : Hub
{
    private readonly AppDbContext _context;

    public TurnosHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task UnirseASala()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "SalaEspera");

        var cola = await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.Pendiente)
            .OrderBy(t => t.FechaCreacion)
            .ToListAsync();

        var enAtencion = await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.EnAtencion)
            .OrderBy(t => t.FechaAtencion)
            .FirstOrDefaultAsync();

        await Clients.Caller.SendAsync("EstadoInicial",
            cola.Select(TurnoService.MapTurno).ToList(),
            enAtencion != null ? TurnoService.MapTurno(enAtencion) : null);
    }

    public async Task UnirseAPanel(string? asesorNombre = null, string? ventanilla = null)
    {
        // Agregar a grupo general de asesores
        await Groups.AddToGroupAsync(Context.ConnectionId, "AsesorPanel");
        
        // Si se proporciona nombre, agregar a grupo específico
        if (!string.IsNullOrEmpty(asesorNombre))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Asesor_{asesorNombre}");
        }

        // Enviar estado actual al asesor que se acaba de conectar
        await ObtenerColaParaLlamador();
    }

    private async Task ObtenerColaParaLlamador()
    {
        var cola = await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.Pendiente)
            .OrderBy(t => t.FechaCreacion)
            .ToListAsync();

        await Clients.Caller.SendAsync("ColaActualizada",
            cola.Select(TurnoService.MapTurno).ToList());
    }

    public async Task RellamarTurno(int turnoId, string? asesorNombre = null, string? ventanilla = null)
    {
        var turno = await _context.Turnos
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == turnoId);

        if (turno != null && turno.Estado == EstadoTurno.EnAtencion)
        {
            // Enviar a la sala de espera para reproducir sonido/TTS
            await Clients.Group("SalaEspera").SendAsync("TurnoLlamado",
                TurnoService.MapTurno(turno));
        }
    }

    public async Task MarcarNoPresentado(int turnoId)
    {
        var turno = await _context.Turnos
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == turnoId);

        if (turno != null && turno.Estado == EstadoTurno.EnAtencion)
        {
            var historial = new HistorialTurno
            {
                TurnoId = turno.Id,
                EstadoAnterior = EstadoTurno.EnAtencion,
                EstadoNuevo = EstadoTurno.NoPresentado,
                Fecha = DateTime.Now,
                Nota = "No se presentó"
            };

            turno.Estado = EstadoTurno.NoPresentado;
            _context.HistorialTurnos.Add(historial);
            await _context.SaveChangesAsync();

            // Notificar a todos
            await Clients.Group("SalaEspera").SendAsync("TurnoNoPresentado", TurnoService.MapTurno(turno));
            await Clients.Group("AsesorPanel").SendAsync("TurnoNoPresentado", TurnoService.MapTurno(turno));
        }
    }
}
