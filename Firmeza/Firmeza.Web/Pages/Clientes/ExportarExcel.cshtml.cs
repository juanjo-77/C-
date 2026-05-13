using Firmeza.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Firmeza.Web.Pages.Clientes
{
    [Authorize(Roles = "Administrador")]
    public class ExportarExcelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ExportarExcelModel(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var clientes = await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Clientes");

            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Nombre";
            ws.Cells[1, 3].Value = "Documento";
            ws.Cells[1, 4].Value = "Correo";
            ws.Cells[1, 5].Value = "Teléfono";
            ws.Cells[1, 6].Value = "Dirección";

            using (var range = ws.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(29, 78, 216));
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            for (int i = 0; i < clientes.Count; i++)
            {
                var c = clientes[i];
                ws.Cells[i + 2, 1].Value = c.Id;
                ws.Cells[i + 2, 2].Value = c.Nombre;
                ws.Cells[i + 2, 3].Value = c.Documento;
                ws.Cells[i + 2, 4].Value = c.Correo;
                ws.Cells[i + 2, 5].Value = c.Telefono;
                ws.Cells[i + 2, 6].Value = c.Direccion;
            }

            ws.Cells.AutoFitColumns();
            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "clientes.xlsx");
        }
    }
}