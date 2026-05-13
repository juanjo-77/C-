using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context) => _context = context;

        public Venta? Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Venta == null) return NotFound();
            return Page();
        }
    }
}