using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Pages.Clientes
{
    [Authorize(Roles = "Administrador")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid) return Page();
                _context.Clientes.Add(Cliente);
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                return Page();
            }
        }
    }
}