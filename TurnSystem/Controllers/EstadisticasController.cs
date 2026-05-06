using Microsoft.AspNetCore.Mvc;
using TurnSystem.Web.Services;

namespace TurnSystem.Web.Controllers;

public class EstadisticasController : Controller
{
    private readonly EstadisticasService _estadisticasService;

    public EstadisticasController(EstadisticasService estadisticasService)
    {
        _estadisticasService = estadisticasService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Datos()
    {
        var datos = await _estadisticasService.ObtenerEstadisticasAsync();
        return Json(datos);
    }
}
