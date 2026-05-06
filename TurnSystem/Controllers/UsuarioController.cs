using Microsoft.AspNetCore.Mvc;
using TurnSystem.Web.Models;
using TurnSystem.Web.Services;
using TurnSystem.Web.Services.Interfaces;

namespace TurnSystem.Web.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITurnoService _turnoService;

    public UsuarioController(IUsuarioService usuarioService, ITurnoService turnoService)
    {
        _usuarioService = usuarioService;
        _turnoService = turnoService;
    }

    // Redirigir a Turno (único flujo de solicitud)
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Turno");
    }

    // Endpoint JSON para Turno/Index.cshtml - OPTIMIZADO
    [HttpPost]
    public async Task<IActionResult> BuscarDocumento([FromBody] BuscarDocRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Documento))
            return Json(new { ok = false, mensaje = "Documento requerido" });

        // UNA sola consulta a la BD
        var (usuario, turnoActivo) = await _turnoService.ObtenerUsuarioYTurnoAsync(request.Documento.Trim());

        if (usuario != null)
        {
            if (turnoActivo != null)
            {
                // Ya tiene turno activo (Pendiente o EnAtencion)
                var turnoData = TurnoService.MapTurno(turnoActivo);
                return Json(new {
                    ok = true,
                    activoExistente = true,
                    turno = turnoData,
                    nombre = usuario.Nombre,
                    puedeCancelar = turnoActivo.Estado == EstadoTurno.Pendiente
                });
            }

            // Usuario existe pero no tiene turno activo → necesita seleccionar servicio/prioridad
            return Json(new { ok = true, existe = true, nombre = usuario.Nombre });
        }

        return Json(new { ok = true, existe = false });
    }

    // Endpoint JSON para Turno/Index.cshtml
    [HttpPost]
    public async Task<IActionResult> RegistrarYGenerar([FromBody] RegistrarYGenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Documento) || string.IsNullOrWhiteSpace(request.Nombre))
            return Json(new { ok = false, mensaje = "Documento y nombre son requeridos" });

        if (string.IsNullOrWhiteSpace(request.Servicio))
            return Json(new { ok = false, mensaje = "Selecciona un servicio" });

        var existente = await _usuarioService.BuscarPorDocumentoAsync(request.Documento.Trim());

        if (existente != null)
        {
            var tieneActivo = await _turnoService.UsuarioTieneActivoAsync(existente.Id);
            if (tieneActivo)
                return Json(new { ok = false, mensaje = "Ya tienes un turno activo" });

            var turnoExistente = await _turnoService.GenerarTurnoAsync(existente.Id, request.Servicio, request.Prioridad);
            var turnoData = TurnoService.MapTurno(turnoExistente);
            return Json(new { ok = true, turno = turnoData });
        }

        var usuario = await _usuarioService.RegistrarAsync(request.Documento.Trim(), request.Nombre.Trim());
        var turnoNuevo = await _turnoService.GenerarTurnoAsync(usuario.Id, request.Servicio, request.Prioridad);
        var turnoData2 = TurnoService.MapTurno(turnoNuevo);
        return Json(new { ok = true, turno = turnoData2 });
    }

    // Cancelar turno pendiente para permitir generar uno nuevo
    [HttpPost]
    public async Task<IActionResult> CancelarTurno([FromBody] CancelarTurnoRequest request)
    {
        if (request?.TurnoId == null)
            return Json(new { ok = false, mensaje = "ID de turno requerido" });

        var resultado = await _turnoService.CancelarTurnoAsync(request.TurnoId);

        if (!resultado)
            return Json(new { ok = false, mensaje = "No se puede cancelar este turno" });

        return Json(new { ok = true });
    }
}

public class CancelarTurnoRequest
{
    public int TurnoId { get; set; }
}

public class BuscarDocRequest
{
    public string Documento { get; set; } = "";
}

public class RegistrarYGenRequest
{
    public string Documento { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Servicio { get; set; } = "";
    public string Prioridad { get; set; } = "Normal";
}