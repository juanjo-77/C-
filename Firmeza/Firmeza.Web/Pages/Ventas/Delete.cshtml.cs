using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Venta? Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (Venta == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.Detalles)
                    .FirstOrDefaultAsync(v => v.Id == Venta!.Id);
                if (venta != null)
                {
                    _context.DetallesVenta.RemoveRange(venta.Detalles);
                    _context.Ventas.Remove(venta);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al eliminar: {ex.Message}");
                return Page();
            }
        }
    }
}