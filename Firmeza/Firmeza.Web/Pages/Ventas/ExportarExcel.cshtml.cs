using Firmeza.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Firmeza.Web.Pages.Ventas
{
    [Authorize(Roles = "Administrador")]
    public class ExportarExcelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ExportarExcelModel(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles).ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Ventas");

            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Fecha";
            ws.Cells[1, 3].Value = "Cliente";
            ws.Cells[1, 4].Value = "Documento";
            ws.Cells[1, 5].Value = "IVA";
            ws.Cells[1, 6].Value = "Total";
            ws.Cells[1, 7].Value = "Productos";

            using (var range = ws.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(29, 78, 216));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            for (int i = 0; i < ventas.Count; i++)
            {
                var v = ventas[i];
                ws.Cells[i + 2, 1].Value = v.Id;
                ws.Cells[i + 2, 2].Value = v.Fecha.ToString("dd/MM/yyyy HH:mm");
                ws.Cells[i + 2, 3].Value = v.Cliente.Nombre;
                ws.Cells[i + 2, 4].Value = v.Cliente.Documento;
                ws.Cells[i + 2, 5].Value = v.IVA;
                ws.Cells[i + 2, 6].Value = v.Total;
                ws.Cells[i + 2, 7].Value = string.Join(", ", v.Detalles.Select(d => $"{d.Producto.Nombre} x{d.Cantidad}"));
            }

            ws.Cells.AutoFitColumns();
            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ventas.xlsx");
        }
    }
}