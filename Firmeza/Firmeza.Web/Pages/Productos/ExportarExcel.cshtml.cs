using Firmeza.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Firmeza.Web.Pages.Productos
{
    [Authorize(Roles = "Administrador")]
    public class ExportarExcelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ExportarExcelModel(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var productos = await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Productos");

            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Nombre";
            ws.Cells[1, 3].Value = "Descripción";
            ws.Cells[1, 4].Value = "Categoría";
            ws.Cells[1, 5].Value = "Precio";
            ws.Cells[1, 6].Value = "Stock";

            using (var range = ws.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(29, 78, 216));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            for (int i = 0; i < productos.Count; i++)
            {
                var p = productos[i];
                ws.Cells[i + 2, 1].Value = p.Id;
                ws.Cells[i + 2, 2].Value = p.Nombre;
                ws.Cells[i + 2, 3].Value = p.Descripcion;
                ws.Cells[i + 2, 4].Value = p.Categoria;
                ws.Cells[i + 2, 5].Value = p.Precio;
                ws.Cells[i + 2, 6].Value = p.Stock;
            }

            ws.Cells.AutoFitColumns();
            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "productos.xlsx");
        }
    }
}