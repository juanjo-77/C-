using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Debes seleccionar un cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public decimal Total { get; set; }
        public decimal IVA { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}