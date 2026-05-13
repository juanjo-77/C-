using Firmeza.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Firmeza.Web.Pages.Productos
{
    [Authorize(Roles = "Administrador")]
    public class ExportarPdfModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ExportarPdfModel(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            var productos = await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(36);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("FIRMEZA — Lista de Productos").Bold().FontSize(16).FontColor("#1d4ed8");
                            row.ConstantItem(150).AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy")).FontColor("#64748b");
                        });
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#e2e8f0");
                    });

                    page.Content().PaddingTop(16).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(40);
                            cols.RelativeColumn(3);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                        });

                        table.Header(h =>
                        {
                            foreach (var t in new[] { "ID", "Nombre", "Categoría", "Descripción", "Precio", "Stock" })
                                h.Cell().Background("#1d4ed8").Padding(6).Text(t).Bold().FontColor(Colors.White);
                        });

                        foreach (var p in productos)
                        {
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(p.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(p.Nombre);
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(p.Categoria);
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(p.Descripcion);
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).AlignRight().Text($"${p.Precio:N2}");
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).AlignCenter().Text(p.Stock.ToString());
                        }
                    });

                    page.Footer().AlignCenter().Text(t =>
                        t.Span("Firmeza — Materiales de Construcción").FontSize(9).FontColor("#94a3b8"));
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "productos.pdf");
        }
    }
}