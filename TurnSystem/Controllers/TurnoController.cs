using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TurnSystem.Web.Controllers;

public class TurnoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    // 🔥 IMPRESIÓN DIRECTA (SIN CUPS)
    [HttpPost]
    public async Task<IActionResult> ImprimirTicket([FromBody] ImprimirRequest request)
    {
        try
        {
            var sb = new StringBuilder();

            // Reset e inicialización
            sb.Append("\x1b\x40");          // ESC @ - reset
            sb.Append("\x1b\x61\x01");      // centrar

            // Encabezado
            sb.Append("\x1b\x21\x00");      // texto normal
            sb.Append("MEDITURNO\n");
            sb.Append("Sistema de Turnos\n");
            sb.Append("------------------------\n");

            // Número de turno (grande y centrado)
            sb.Append("\x1b\x21\x30");      // doble altura
            sb.Append($"{request.Ticket}\n");
            sb.Append("\x1b\x21\x00");      // normal

            // Servicio y prioridad
            if (!string.IsNullOrEmpty(request.Servicio))
            {
                sb.Append($"Servicio: {request.Servicio}\n");
            }
            if (!string.IsNullOrEmpty(request.Prioridad) && request.Prioridad != "Normal")
            {
                sb.Append($"Prioridad: {request.Prioridad}\n");
            }

            sb.Append("------------------------\n");

            // Datos del paciente
            sb.Append("\x1b\x61\x00");      // alinear izquierda
            sb.Append($"Paciente: {request.Nombre}\n");
            if (!string.IsNullOrEmpty(request.Documento))
            {
                sb.Append($"Doc: {request.Documento}\n");
            }

            // Fecha y hora
            var ahora = DateTime.Now;
            sb.Append($"Fecha: {ahora:dd/MM/yyyy HH:mm}\n");

            sb.Append("------------------------\n");
            sb.Append("\x1b\x61\x01");      // centrar

            // Código de barras simulado (texto)
            sb.Append($"[{request.Ticket}]\n");

            // Mensaje final
            sb.Append("\x1b\x21\x01");      // negrita
            sb.Append("Por favor espere\n");
            sb.Append("\x1b\x21\x00");      // normal
            sb.Append("Su turno será anunciado\n");

            // Cortar papel (opcional, depende de la impresora)
            sb.Append("\n\n\n");
            sb.Append("\x1d\x56\x00");      // GS V - corte total

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());

            // 🔥 impresión directa al dispositivo
            await System.IO.File.WriteAllBytesAsync("/dev/usb/lp0", bytes);

            return Json(new { ok = true });
        }
        catch (Exception ex)
        {
            return Json(new { ok = false, mensaje = ex.Message });
        }
    }
}

public class ImprimirRequest
{
    public string Ticket { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Servicio { get; set; } = "";
    public string Prioridad { get; set; } = "Normal";
}