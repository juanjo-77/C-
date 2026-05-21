using Firmeza.API.Data;
using Firmeza.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ProductosController(ApiDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Productos.OrderBy(p => p.Nombre).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _context.Productos.FindAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest();
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Productos.FindAsync(id);
            if (p == null) return NotFound();
            _context.Productos.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}