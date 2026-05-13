using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public List<Venta> Ventas { get; set; } = new();

        public async Task OnGetAsync()
        {
            Ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }
    }
}