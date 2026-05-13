using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Pages.Clientes
{
    [Authorize(Roles = "Administrador")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Cliente? Cliente { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Cliente = await _context.Clientes.FindAsync(id);
            if (Cliente == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(Cliente!.Id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
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