using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;

namespace Firmeza.Web.Pages.Importar
{
    [Authorize(Roles = "Administrador")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context) => _context = context;

        public ResultadoImportacion? Resultado { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile archivo)
        {
            Resultado = new ResultadoImportacion();

            if (archivo == null || archivo.Length == 0)
            {
                Resultado.Errores.Add("No se seleccionó ningún archivo.");
                return Page();
            }

            if (!archivo.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                Resultado.Errores.Add("Solo se aceptan archivos .xlsx");
                return Page();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Guardar temporalmente en disco para evitar problemas de stream
            var tempPath = Path.GetTempFileName() + ".xlsx";
            try
            {
                using (var fs = new FileStream(tempPath, FileMode.Create))
                {
                    await archivo.CopyToAsync(fs);
                }

                using var package = new ExcelPackage(new FileInfo(tempPath));

                if (package.Workbook.Worksheets.Count == 0)
                {
                    Resultado.Errores.Add("El archivo no contiene hojas de cálculo.");
                    return Page();
                }

                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null) continue;

                    var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var header = worksheet.Cells[1, col].Text.Trim().ToLower()
                            .Replace("ó", "o").Replace("é", "e").Replace("í", "i")
                            .Replace("á", "a").Replace("ú", "u");
                        if (!string.IsNullOrEmpty(header))
                            headers[header] = col;
                    }

                    bool esProducto = headers.ContainsKey("precio") || headers.ContainsKey("stock") || headers.ContainsKey("categoria");
                    bool esCliente = headers.ContainsKey("documento") || headers.ContainsKey("correo") || headers.ContainsKey("telefono");

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string GetVal(string key) =>
                            headers.ContainsKey(key) ? worksheet.Cells[row, headers[key]].Text.Trim() : "";

                        // Fila completamente vacía
                        bool filaVacia = true;
                        for (int c = 1; c <= worksheet.Dimension.End.Column; c++)
                        {
                            if (!string.IsNullOrEmpty(worksheet.Cells[row, c].Text.Trim()))
                            { filaVacia = false; break; }
                        }
                        if (filaVacia) continue;

                        if (esProducto)
                        {
                            var nombre = GetVal("nombre");
                            var precioStr = GetVal("precio").Replace(".", "").Replace(",", ".");
                            var stockStr = GetVal("stock");

                            if (string.IsNullOrEmpty(nombre))
                            {
                                Resultado.Errores.Add($"Hoja '{worksheet.Name}' Fila {row}: nombre vacío, se omite.");
                                continue;
                            }
                            if (!decimal.TryParse(precioStr, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out decimal precio) || precio <= 0)
                            {
                                Resultado.Errores.Add($"Hoja '{worksheet.Name}' Fila {row}: precio inválido '{GetVal("precio")}' para '{nombre}'.");
                                continue;
                            }
                            int.TryParse(stockStr, out int stock);

                            var existente = _context.Productos
                                .FirstOrDefault(p => p.Nombre.ToLower() == nombre.ToLower());

                            if (existente != null)
                            {
                                existente.Precio = precio;
                                if (stock > 0) existente.Stock = stock;
                                var desc = GetVal("descripcion");
                                var cat = GetVal("categoria");
                                if (!string.IsNullOrEmpty(desc)) existente.Descripcion = desc;
                                if (!string.IsNullOrEmpty(cat)) existente.Categoria = cat;
                                _context.Productos.Update(existente);
                            }
                            else
                            {
                                _context.Productos.Add(new Producto
                                {
                                    Nombre = nombre,
                                    Descripcion = GetVal("descripcion") is { Length: > 0 } d ? d : "Sin descripción",
                                    Categoria = GetVal("categoria") is { Length: > 0 } c ? c : "General",
                                    Precio = precio,
                                    Stock = stock
                                });
                            }
                            Resultado.ProductosInsertados++;
                        }

                        if (esCliente)
                        {
                            var nombre = GetVal("nombre");
                            var documento = GetVal("documento");
                            var correo = GetVal("correo");
                            var telefono = GetVal("telefono");
                            var direccion = GetVal("direccion");

                            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(documento))
                            {
                                Resultado.Errores.Add($"Hoja '{worksheet.Name}' Fila {row}: nombre o documento vacío, se omite.");
                                continue;
                            }

                            var existente = _context.Clientes
                                .FirstOrDefault(c => c.Documento == documento);

                            if (existente != null)
                            {
                                existente.Nombre = nombre;
                                if (!string.IsNullOrEmpty(correo)) existente.Correo = correo;
                                if (!string.IsNullOrEmpty(telefono)) existente.Telefono = telefono;
                                if (!string.IsNullOrEmpty(direccion)) existente.Direccion = direccion;
                                _context.Clientes.Update(existente);
                            }
                            else
                            {
                                _context.Clientes.Add(new Cliente
                                {
                                    Nombre = nombre,
                                    Documento = documento,
                                    Correo = !string.IsNullOrEmpty(correo) ? correo : "sin@correo.com",
                                    Telefono = !string.IsNullOrEmpty(telefono) ? telefono : "0000000",
                                    Direccion = !string.IsNullOrEmpty(direccion) ? direccion : "Sin dirección"
                                });
                            }
                            Resultado.ClientesInsertados++;
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Resultado.Errores.Add($"Error al procesar el archivo: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);
            }

            return Page();
        }

        public class ResultadoImportacion
        {
            public int ProductosInsertados { get; set; }
            public int ClientesInsertados { get; set; }
            public List<string> Errores { get; set; } = new();
        }
    }
}