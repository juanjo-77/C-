using System.ComponentModel.DataAnnotations;

namespace Firmeza.Web.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento es obligatorio")]
        [RegularExpression(@"^\d{7,12}$", ErrorMessage = "El documento debe tener entre 7 y 12 dígitos")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El teléfono debe tener entre 7 y 15 dígitos")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;
    }
}