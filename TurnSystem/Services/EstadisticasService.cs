using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Data;
using TurnSystem.Web.Models;

namespace TurnSystem.Web.Services;

public class EstadisticasService
{
    private readonly AppDbContext _context;

    public EstadisticasService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> ObtenerEstadisticasAsync()
    {
        var hoy = DateTime.Today;
        var turnosHoy = await _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.FechaCreacion.Date == hoy)
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();

        var totalTurnos = turnosHoy.Count;
        var finalizados = turnosHoy.Count(t => t.Estado == EstadoTurno.Finalizado);

        var turnosConTiempo = turnosHoy
            .Where(t => t.FechaAtencion.HasValue && t.FechaFin.HasValue)
            .ToList();

        double tiempoPromedio = 0;
        if (turnosConTiempo.Count > 0)
        {
            var minutos = turnosConTiempo
                .Select(t => (t.FechaFin.Value - t.FechaAtencion.Value).TotalMinutes)
                .Average();
            tiempoPromedio = Math.Round(minutos, 1);
        }

var porAsesor = turnosHoy
    .Where(t => t.Estado == EstadoTurno.Finalizado || t.Estado == EstadoTurno.NoPresentado || t.Estado == EstadoTurno.Cancelado)
    .GroupBy(t => t.Estado == EstadoTurno.NoPresentado ? "No presentó" :
                   t.Estado == EstadoTurno.Cancelado ? "Cancelado" : "Atendido")
    .Select(g => new { label = g.Key, valor = g.Count() })
    .ToList();

        var porServicio = turnosHoy
            .GroupBy(t => t.Servicio ?? "General")
            .Select(g => new { label = g.Key, valor = g.Count() })
            .OrderByDescending(x => x.valor)
            .ToList();

        var turnos = turnosHoy.Select(t => new
        {
            ticket = t.Ticket,
            paciente = t.Usuario?.Nombre ?? "—",
            servicio = t.Servicio ?? "General",
            prioridad = t.Prioridad ?? "Normal",
            asesor = "—",
            estado = t.Estado.ToString(),
            hora = t.FechaCreacion.ToString("HH:mm")
        }).ToList();

        return new
        {
            totalTurnos,
            finalizados,
            tiempoPromedio = tiempoPromedio > 0 ? tiempoPromedio : (double?)null,
            asesoresActivos = 1,
            porAsesor,
            porServicio,
            turnos
        };
    }
}
