using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Pages.Productos
{
    [Authorize(Roles = "Administrador")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto? Producto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.Productos.FindAsync(id);
            if (Producto == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var producto = await _context.Productos.FindAsync(Producto!.Id);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
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
