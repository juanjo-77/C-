namespace exploracion_espacial.Models
{
    public class Nave
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public int CapacidadTripulacion { get; set; }
        public string Estado { get; set; }         // operativa, en mantenimiento, retirada

        // Propiedad de navegación
        public ICollection<Mision> Misiones { get; set; }
    }
}