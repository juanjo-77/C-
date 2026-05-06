using Microsoft.AspNetCore.Mvc;

namespace TurnSystem.Web.Controllers;

public class SalaController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}