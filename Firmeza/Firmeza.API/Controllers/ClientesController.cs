using Firmeza.API.Data;
using Firmeza.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ClientesController(ApiDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}