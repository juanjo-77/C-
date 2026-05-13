using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages.Productos
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Producto> Productos { get; set; } = new();
        public string Buscar { get; set; } = string.Empty;

        public async Task OnGetAsync(string buscar)
        {
            Buscar = buscar ?? string.Empty;
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(Buscar))
                query = query.Where(p => p.Nombre.Contains(Buscar) || p.Categoria.Contains(Buscar));

            Productos = await query.OrderBy(p => p.Nombre).ToListAsync();
        }
    }
}