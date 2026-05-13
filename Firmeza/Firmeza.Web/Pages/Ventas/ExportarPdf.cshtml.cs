using Firmeza.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class ExportarPdfModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ExportarPdfModel(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

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
                            row.RelativeItem().Text("FIRMEZA — Reporte de Ventas").Bold().FontSize(16).FontColor("#1d4ed8");
                            row.ConstantItem(150).AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy")).FontColor("#64748b");
                        });
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#e2e8f0");
                    });

                    page.Content().PaddingTop(16).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(35);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                        });

                        table.Header(h =>
                        {
                            foreach (var t in new[] { "#", "Fecha", "Cliente", "IVA", "Total" })
                                h.Cell().Background("#1d4ed8").Padding(6).Text(t).Bold().FontColor(Colors.White);
                        });

                        foreach (var v in ventas)
                        {
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(v.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(v.Fecha.ToString("dd/MM/yyyy"));
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).Text(v.Cliente.Nombre);
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).AlignRight().Text($"${v.IVA:N2}");
                            table.Cell().BorderBottom(1).BorderColor("#f1f5f9").Padding(5).AlignRight().Text($"${v.Total:N2}");
                        }
                    });

                    page.Footer().AlignCenter().Text(t =>
                        t.Span("Firmeza — Materiales de Construcción").FontSize(9).FontColor("#94a3b8"));
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "ventas.pdf");
        }
    }
}