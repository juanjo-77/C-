using Microsoft.AspNetCore.Mvc;

namespace TurnSystem.Web.Controllers;

public class AuthController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Registro() => View();
}
