using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Web.Pages
{
    [Authorize(Roles = "Administrador")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalVentas { get; set; }
        public List<Venta> UltimasVentas { get; set; } = new();
        public List<Producto> ProductosBajoStock { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalProductos = await _context.Productos.CountAsync();
            TotalClientes = await _context.Clientes.CountAsync();
            TotalVentas = await _context.Ventas.CountAsync();

            UltimasVentas = await _context.Ventas
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.Fecha)
                .Take(5)
                .ToListAsync();

            ProductosBajoStock = await _context.Productos
                .Where(p => p.Stock < 10)
                .OrderBy(p => p.Stock)
                .Take(5)
                .ToListAsync();
        }
    }
}