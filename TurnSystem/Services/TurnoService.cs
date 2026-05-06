using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Data;
using TurnSystem.Web.Hubs;
using TurnSystem.Web.Models;
using TurnSystem.Web.Services.Interfaces;

namespace TurnSystem.Web.Services;

public class TurnoService : ITurnoService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<TurnosHub> _hub;

    public TurnoService(AppDbContext context, IHubContext<TurnosHub> hub)
    {
        _context = context;
        _hub     = hub;
    }

    public async Task<List<Turno>> ObtenerPorEstadoAsync(EstadoTurno estado)
    {
        return await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == estado)
            .OrderBy(t => t.FechaCreacion)
            .ToListAsync();
    }

public async Task<bool> UsuarioTieneActivoAsync(int usuarioId)
{
    return await _context.Turnos.AnyAsync(t =>
        t.UsuarioId == usuarioId &&
        t.Estado != EstadoTurno.Finalizado &&
        t.Estado != EstadoTurno.NoPresentado &&
        t.Estado != EstadoTurno.Cancelado);
}

    public async Task<Turno?> ObtenerTurnoActivoAsync(int usuarioId)
    {
        return await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.UsuarioId == usuarioId
                     && t.Estado != EstadoTurno.Finalizado
                     && t.Estado != EstadoTurno.NoPresentado)
            .OrderByDescending(t => t.FechaCreacion)
            .FirstOrDefaultAsync();
    }

    // Optimización: Obtener usuario y turno activo en UNA sola consulta
    public async Task<(Usuario? usuario, Turno? turnoActivo)> ObtenerUsuarioYTurnoAsync(string documento)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Turnos)
            .FirstOrDefaultAsync(u => u.Documento == documento);

        if (usuario == null) return (null, null);

var turnoActivo = usuario.Turnos
    .Where(t => t.Estado != EstadoTurno.Finalizado && 
                t.Estado != EstadoTurno.NoPresentado &&
                t.Estado != EstadoTurno.Cancelado)
    .OrderByDescending(t => t.FechaCreacion)
    .FirstOrDefault();

        return (usuario, turnoActivo);
    }

    public async Task<Turno> GenerarTurnoAsync(int usuarioId, string? servicio = null, string? prioridad = null)
    {
        var ultimoHoy = await _context.Turnos
            .Where(t => t.FechaCreacion.Date == DateTime.Today)
            .OrderByDescending(t => t.Id)
            .Select(t => t.Ticket)
            .FirstOrDefaultAsync();

        int siguiente = 1;
        if (ultimoHoy != null && ultimoHoy.Length > 2)
        {
            var parteNumerica = ultimoHoy.Substring(2); // quita "A-"
            if (int.TryParse(parteNumerica, out int ultimo))
                siguiente = ultimo + 1;
        }

        var ticket = $"A-{siguiente:D3}";

        var turno = new Turno
        {
            Ticket        = ticket,
            UsuarioId     = usuarioId,
            Estado        = EstadoTurno.Pendiente,
            FechaCreacion = DateTime.Now,
            Servicio      = servicio ?? "General",
            Prioridad     = prioridad ?? "Normal"
        };

        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();

        await _context.Entry(turno).Reference(t => t.Usuario).LoadAsync();

        // Notificar a la sala de espera y asesores con el turno completo
        await _hub.Clients.Group("SalaEspera").SendAsync("TurnoAgregado", MapTurno(turno));
        await _hub.Clients.Group("AsesorPanel").SendAsync("TurnoAgregado", MapTurno(turno));

        return turno;
    }

    public async Task<Turno?> LlamarSiguienteAsync(string? asesorNombre = null, string? ventanilla = null)
    {
        var pendientes = await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.Pendiente)
            .ToListAsync();

        var turno = pendientes
            .OrderBy(t => GetPrioridadOrden(t))
            .ThenBy(t => t.FechaCreacion)
            .FirstOrDefault();

        if (turno == null) return null;

        var historial = new HistorialTurno
        {
            TurnoId        = turno.Id,
            EstadoAnterior = EstadoTurno.Pendiente,
            EstadoNuevo    = EstadoTurno.EnAtencion,
            Fecha          = DateTime.Now,
            Nota           = $"Turno llamado por {asesorNombre ?? "—"} (Ventanilla {ventanilla ?? "1"})"
        };

        turno.Estado        = EstadoTurno.EnAtencion;
        turno.FechaAtencion = DateTime.Now;
        turno.AsesorNombre  = asesorNombre;
        turno.Ventanilla    = ventanilla;

        _context.HistorialTurnos.Add(historial);
        await _context.SaveChangesAsync();

        // Notificar a todos: sala de espera y paneles de asesores
        await _hub.Clients.Group("SalaEspera").SendAsync("TurnoLlamado", MapTurno(turno));
        await _hub.Clients.Group("AsesorPanel").SendAsync("TurnoLlamado", MapTurno(turno));

        return turno;
    }

    private static int GetPrioridadOrden(Turno t) => t.Prioridad switch
    {
        "Urgencia" => 0,
        "VIP"      => 1,
        _          => 2
    };

    public async Task<bool> FinalizarAtencionAsync(int turnoId, string? comentario)
    {
        var turno = await _context.Turnos
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == turnoId);

        if (turno == null) return false;

        var historial = new HistorialTurno
        {
            TurnoId        = turno.Id,
            EstadoAnterior = EstadoTurno.EnAtencion,
            EstadoNuevo    = EstadoTurno.Finalizado,
            Fecha          = DateTime.Now,
            Nota           = comentario
        };

        turno.Estado     = EstadoTurno.Finalizado;
        turno.FechaFin   = DateTime.Now;
        turno.Comentario = comentario;

        _context.HistorialTurnos.Add(historial);
        await _context.SaveChangesAsync();

        // Notificar a todos los clientes
        await _hub.Clients.Group("SalaEspera").SendAsync("TurnoFinalizado", MapTurno(turno));
        await _hub.Clients.Group("AsesorPanel").SendAsync("TurnoFinalizado", MapTurno(turno));

        return true;
    }

    public async Task<bool> MarcarNoPresentadoAsync(int turnoId)
    {
        var turno = await _context.Turnos
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == turnoId);

        if (turno == null) return false;

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

        await _hub.Clients.Group("SalaEspera").SendAsync("TurnoNoPresentado", MapTurno(turno));
        await _hub.Clients.Group("AsesorPanel").SendAsync("TurnoNoPresentado", MapTurno(turno));

        return true;
    }

    public async Task<bool> CancelarTurnoAsync(int turnoId)
    {
        var turno = await _context.Turnos
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == turnoId);

        if (turno == null || turno.Estado != EstadoTurno.Pendiente)
            return false;

        var historial = new HistorialTurno
        {
            TurnoId = turno.Id,
            EstadoAnterior = EstadoTurno.Pendiente,
            EstadoNuevo = EstadoTurno.Cancelado,
            Fecha = DateTime.Now,
            Nota = "Cancelado por el usuario"
        };

        turno.Estado = EstadoTurno.Cancelado;
        _context.HistorialTurnos.Add(historial);
        await _context.SaveChangesAsync();

        // Notificar a todos
        await _hub.Clients.Group("SalaEspera").SendAsync("TurnoCancelado", MapTurno(turno));
        await _hub.Clients.Group("AsesorPanel").SendAsync("TurnoCancelado", MapTurno(turno));

        return true;
    }

    public static object MapTurno(Turno t) => new
    {
        id            = t.Id,
        ticket        = t.Ticket,
        estado        = t.Estado.ToString(),
        fechaCreacion = t.FechaCreacion,
        nombreUsuario = t.Usuario?.Nombre,
        documento     = t.Usuario?.Documento,
        paciente      = t.Usuario?.Nombre ?? "—",
        servicio      = t.Servicio ?? "General",
        prioridad     = t.Prioridad ?? "Normal",
        asesor        = t.AsesorNombre ?? "—",
        ventanilla    = t.Ventanilla ?? "—",
        horaFin       = t.FechaFin?.ToString("HH:mm") ?? ""
    };
}