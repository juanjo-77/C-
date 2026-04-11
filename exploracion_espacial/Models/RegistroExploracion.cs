namespace exploracion_espacial.Models
{
    public class RegistroExploracion
    {
        public int Id { get; set; }
        public string PlanetaDestino { get; set; }
        public string Descripcion { get; set; }
        public string NivelRiesgo { get; set; }

        // Claves foráneas
        public int MisionId { get; set; }

        // Propiedad de navegación
        public Mision Mision { get; set; }
    }
}
