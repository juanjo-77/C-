using Firmeza.Web.Data;
using Firmeza.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Tests
{
    public class ProductoServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CrearProducto_DebeGuardarCorrectamente()
        {
            // Arrange
            var context = GetDbContext();
            var producto = new Producto
            {
                Nombre = "Cemento Gris",
                Descripcion = "Bolsa 50kg",
                Categoria = "Cemento",
                Precio = 25000,
                Stock = 100
            };

            // Act
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            // Assert
            var resultado = await context.Productos.FirstOrDefaultAsync(p => p.Nombre == "Cemento Gris");
            Assert.NotNull(resultado);
            Assert.Equal(25000, resultado.Precio);
            Assert.Equal(100, resultado.Stock);
        }

        [Fact]
        public async Task EliminarProducto_DebeQuitarloDelaBase()
        {
            // Arrange
            var context = GetDbContext();
            var producto = new Producto
            {
                Nombre = "Arena Fina",
                Descripcion = "Bulto 40kg",
                Categoria = "Arena",
                Precio = 8000,
                Stock = 50
            };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            // Act
            context.Productos.Remove(producto);
            await context.SaveChangesAsync();

            // Assert
            var resultado = await context.Productos.FirstOrDefaultAsync(p => p.Nombre == "Arena Fina");
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ActualizarStock_DebeReflejarCambio()
        {
            // Arrange
            var context = GetDbContext();
            var producto = new Producto
            {
                Nombre = "Ladrillo",
                Descripcion = "Unidad",
                Categoria = "Ladrillo",
                Precio = 500,
                Stock = 200
            };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            // Act
            producto.Stock -= 30;
            context.Productos.Update(producto);
            await context.SaveChangesAsync();

            // Assert
            var resultado = await context.Productos.FindAsync(producto.Id);
            Assert.Equal(170, resultado!.Stock);
        }

        [Fact]
        public async Task CrearCliente_DebeGuardarCorrectamente()
        {
            // Arrange
            var context = GetDbContext();
            var cliente = new Cliente
            {
                Nombre = "Juan Pérez",
                Documento = "1234567890",
                Correo = "juan@correo.com",
                Telefono = "3001234567",
                Direccion = "Calle 123"
            };

            // Act
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            // Assert
            var resultado = await context.Clientes.FirstOrDefaultAsync(c => c.Documento == "1234567890");
            Assert.NotNull(resultado);
            Assert.Equal("Juan Pérez", resultado.Nombre);
        }

        [Fact]
        public async Task CrearVenta_DebeCalcularTotalConIVA()
        {
            // Arrange
            var context = GetDbContext();

            var cliente = new Cliente
            {
                Nombre = "María López",
                Documento = "9876543210",
                Correo = "maria@correo.com",
                Telefono = "3109876543",
                Direccion = "Carrera 45"
            };
            context.Clientes.Add(cliente);

            var producto = new Producto
            {
                Nombre = "Varilla",
                Descripcion = "Varilla 1/2",
                Categoria = "Hierro",
                Precio = 30000,
                Stock = 20
            };
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            // Act
            decimal precioUnitario = 30000;
            int cantidad = 2;
            decimal subtotal = precioUnitario * cantidad;
            decimal iva = subtotal * 0.19m;
            decimal total = subtotal + iva;

            var venta = new Venta
            {
                ClienteId = cliente.Id,
                Fecha = DateTime.Now,
                IVA = iva,
                Total = total,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = producto.Id,
                        Cantidad = cantidad,
                        PrecioUnitario = precioUnitario
                    }
                }
            };
            context.Ventas.Add(venta);
            await context.SaveChangesAsync();

            // Assert
            var resultado = await context.Ventas.FindAsync(venta.Id);
            Assert.NotNull(resultado);
            Assert.Equal(71400, resultado.Total);   // 60000 + 19% = 71400
            Assert.Equal(11400, resultado.IVA);
        }
    }
}