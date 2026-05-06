using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Data;
using TurnSystem.Web.Models;
using TurnSystem.Web.Services;
using TurnSystem.Web.Services.Interfaces;

namespace TurnSystem.Web.Controllers;

public class AsesorController : Controller
{
    private readonly ITurnoService _turnoService;
    private readonly AppDbContext _context;

    public AsesorController(ITurnoService turnoService, AppDbContext context)
    {
        _turnoService = turnoService;
        _context = context;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> ObtenerTurnos([FromQuery] string? asesorNombre = null, [FromQuery] string? ventanilla = null)
    {
        var pendientes  = await _turnoService.ObtenerPorEstadoAsync(EstadoTurno.Pendiente);
        
        // Filtrar turnos en atención por asesor si se proporcionan los datos
        var enAtencionQuery = _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.EnAtencion);
            
        if (!string.IsNullOrEmpty(asesorNombre))
        {
            enAtencionQuery = enAtencionQuery.Where(t => t.AsesorNombre == asesorNombre);
        }
        
        var enAtencion = await enAtencionQuery.OrderBy(t => t.FechaAtencion).ToListAsync();
        
        // Filtrar finalizados por asesor si se proporcionan los datos
        var finalizadosQuery = _context.Turnos
            .Include(t => t.Usuario)
            .Where(t => t.Estado == EstadoTurno.Finalizado);
            
        if (!string.IsNullOrEmpty(asesorNombre))
        {
            finalizadosQuery = finalizadosQuery.Where(t => t.AsesorNombre == asesorNombre);
        }
        
        var finalizados = await finalizadosQuery
            .OrderByDescending(t => t.FechaFin)
            .Take(50) // Limitar a los últimos 50
            .ToListAsync();

        return Json(new
        {
            ok = true,
            data = new
            {
                Pendiente  = pendientes.Select(t  => TurnoService.MapTurno(t)).ToList(),
                EnAtencion = enAtencion.Select(t  => TurnoService.MapTurno(t)).ToList(),
                Finalizado = finalizados.Select(t => TurnoService.MapTurno(t)).ToList()
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> LlamarSiguiente([FromBody] LlamarSiguienteRequest request)
    {
        var turno = await _turnoService.LlamarSiguienteAsync(request?.AsesorNombre, request?.Ventanilla);

        if (turno == null)
            return Json(new { ok = false, mensaje = "No hay turnos pendientes" });

        return Json(new { ok = true, turno = TurnoService.MapTurno(turno) });
    }

    [HttpPost]
    public async Task<IActionResult> Finalizar([FromBody] FinalizarRequest request)
    {
        var resultado = await _turnoService.FinalizarAtencionAsync(request.TurnoId, request.Comentario);

        if (!resultado)
            return Json(new { ok = false, mensaje = "Error al finalizar el turno" });

        return Json(new { ok = true });
    }

    [HttpPost]
    public async Task<IActionResult> NoPresentado([FromBody] NoPresentadoRequest request)
    {
        var resultado = await _turnoService.MarcarNoPresentadoAsync(request.TurnoId);

        if (!resultado)
            return Json(new { ok = false, mensaje = "Error al marcar como no presentado" });

        return Json(new { ok = true });
    }
}

public class NoPresentadoRequest
{
    public int TurnoId { get; set; }
}

public class FinalizarRequest
{
    public int TurnoId { get; set; }
    public string? Comentario { get; set; }
}

public class LlamarSiguienteRequest
{
    public string? AsesorNombre { get; set; }
    public string? Ventanilla { get; set; }
}