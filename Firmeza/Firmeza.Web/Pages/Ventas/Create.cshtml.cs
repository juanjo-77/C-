using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Cliente> Clientes { get; set; } = new();
        public List<Producto> Productos { get; set; } = new();

        [BindProperty] public int ClienteId { get; set; }
        [BindProperty] public string DetallesJson { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Clientes = await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync();
            Productos = await _context.Productos.Where(p => p.Stock > 0).OrderBy(p => p.Nombre).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Clientes = await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync();
            Productos = await _context.Productos.Where(p => p.Stock > 0).OrderBy(p => p.Nombre).ToListAsync();

            try
            {
                if (ClienteId == 0)
                {
                    ModelState.AddModelError("", "Debes seleccionar un cliente.");
                    return Page();
                }

                if (string.IsNullOrEmpty(DetallesJson))
                {
                    ModelState.AddModelError("", "Debes agregar al menos un producto.");
                    return Page();
                }

                List<DetalleItem>? items;
                try
                {
                    items = JsonSerializer.Deserialize<List<DetalleItem>>(DetallesJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch
                {
                    ModelState.AddModelError("", "Error al procesar los productos seleccionados.");
                    return Page();
                }

                if (items == null || items.Count == 0)
                {
                    ModelState.AddModelError("", "Debes agregar al menos un producto.");
                    return Page();
                }

                var venta = new Venta
                {
                    ClienteId = ClienteId,
                    Fecha = DateTime.UtcNow,
                    Detalles = new List<DetalleVenta>()
                };

                foreach (var item in items)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", $"Producto con ID {item.ProductoId} no encontrado.");
                        return Page();
                    }
                    if (producto.Stock < item.Cantidad)
                    {
                        ModelState.AddModelError("", $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}");
                        return Page();
                    }
                    producto.Stock -= item.Cantidad;
                    venta.Detalles.Add(new DetalleVenta
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio
                    });
                }

                decimal subtotal = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
                venta.IVA = Math.Round(subtotal * 0.19m, 2);
                venta.Total = Math.Round(subtotal + venta.IVA, 2);

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                await GenerarReciboPdf(venta.Id);

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                return Page();
            }
        }

        private async Task GenerarReciboPdf(int ventaId)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == ventaId);

            if (venta == null) return;

            var recibosPath = Path.Combine(_env.WebRootPath, "recibos");
            Directory.CreateDirectory(recibosPath);
            var filePath = Path.Combine(recibosPath, $"recibo_{venta.Id}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("FIRMEZA").Bold().FontSize(22).FontColor("#1d4ed8");
                                c.Item().Text("Materiales de Construcción").FontSize(10).FontColor("#64748b");
                            });
                            row.ConstantItem(160).AlignRight().Column(c =>
                            {
                                c.Item().Text($"RECIBO #{venta.Id:D5}").Bold().FontSize(14);
                                c.Item().Text(venta.Fecha.ToString("dd/MM/yyyy HH:mm")).FontSize(10).FontColor("#64748b");
                            });
                        });
                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e2e8f0");
                    });

                    page.Content().PaddingTop(20).Column(col =>
                    {
                        col.Item().Background("#f8fafc").Padding(12).Column(c =>
                        {
                            c.Item().Text("DATOS DEL CLIENTE").Bold().FontSize(10).FontColor("#64748b");
                            c.Item().PaddingTop(4).Text(venta.Cliente.Nombre).Bold().FontSize(13);
                            c.Item().Text($"Documento: {venta.Cliente.Documento}").FontSize(10);
                            c.Item().Text($"Correo: {venta.Cliente.Correo}").FontSize(10);
                            c.Item().Text($"Teléfono: {venta.Cliente.Telefono}").FontSize(10);
                        });

                        col.Item().PaddingTop(16).Text("DETALLE DE PRODUCTOS").Bold().FontSize(10).FontColor("#64748b");
                        col.Item().PaddingTop(6).Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(1);
                                cols.RelativeColumn(1);
                                cols.RelativeColumn(1);
                            });
                            table.Header(h =>
                            {
                                foreach (var t in new[] { "Producto", "Precio Unit.", "Cant.", "Subtotal" })
                                    h.Cell().Background("#1d4ed8").Padding(6).Text(t).Bold().FontColor(Colors.White).FontSize(10);
                            });
                            foreach (var d in venta.Detalles)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).Text(d.Producto.Nombre);
                                table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).AlignRight().Text($"${d.PrecioUnitario:N2}");
                                table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).AlignCenter().Text(d.Cantidad.ToString());
                                table.Cell().BorderBottom(1).BorderColor("#e2e8f0").Padding(6).AlignRight().Text($"${d.Subtotal:N2}");
                            }
                        });

                        decimal subtotal = venta.Detalles.Sum(d => d.Subtotal);
                        col.Item().PaddingTop(12).AlignRight().Column(totales =>
                        {
                            totales.Item().Row(r => { r.ConstantItem(130).Text("Subtotal:").FontColor("#64748b"); r.ConstantItem(100).AlignRight().Text($"${subtotal:N2}"); });
                            totales.Item().Row(r => { r.ConstantItem(130).Text("IVA (19%):").FontColor("#64748b"); r.ConstantItem(100).AlignRight().Text($"${venta.IVA:N2}"); });
                            totales.Item().PaddingTop(4).Row(r => { r.ConstantItem(130).Text("TOTAL:").Bold().FontSize(14); r.ConstantItem(100).AlignRight().Text($"${venta.Total:N2}").Bold().FontSize(14).FontColor("#1d4ed8"); });
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                        t.Span("Firmeza — Materiales de Construcción | Gracias por su compra").FontSize(9).FontColor("#94a3b8"));
                });
            }).GeneratePdf(filePath);
        }

        public class DetalleItem
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }
    }
}