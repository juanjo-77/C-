using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages.Clientes
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public List<Cliente> Clientes { get; set; } = new();
        public string Buscar { get; set; } = string.Empty;

        public async Task OnGetAsync(string buscar)
        {
            Buscar = buscar ?? string.Empty;
            var query = _context.Clientes.AsQueryable();
            if (!string.IsNullOrEmpty(Buscar))
                query = query.Where(c => c.Nombre.Contains(Buscar) || c.Documento.Contains(Buscar));
            Clientes = await query.OrderBy(c => c.Nombre).ToListAsync();
        }
    }
}