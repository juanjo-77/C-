using Microsoft.AspNetCore.Mvc;
using CrudMVC.Models;
using CrudMVC.Services;

namespace CrudMVC.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteService _service;

        public ClientesController(ClienteService service)
        {
            _service = service;
        }
//===========================================================================
        // GET: /Clientes
        public IActionResult Index()
        {
            var clientes = _service.GetAll();
            return View(clientes);
        }
//===========================================================================
        // GET: /Clientes/Create
        public IActionResult Create() => View();

        // POST: /Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            _service.Create(cliente);
            TempData["Mensaje"] = "Cliente creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
//===========================================================================
        // GET: /Clientes/Edit/5
        public IActionResult Edit(int id)
        {
            var cliente = _service.GetById(id);
            if (cliente is null) return NotFound();
            return View(cliente);
        }

        // POST: /Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            if (!ModelState.IsValid) return View(cliente);

            if (!_service.Update(cliente)) return NotFound();

            TempData["Mensaje"] = "Cliente actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
//===========================================================================
        // GET: /Clientes/Delete/5
        public IActionResult Delete(int id)
        {
            var cliente = _service.GetById(id);
            if (cliente is null) return NotFound();
            return View(cliente);
        }

        // POST: /Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            TempData["Mensaje"] = "Cliente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}