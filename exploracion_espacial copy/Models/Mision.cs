namespace exploracion_espacial.Models
{
    public class Mision
    {
        public int Id { get; set; }
        public string NombreMision { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Estado { get; set; }         // planificada, en curso, completada, cancelada


        // Claves foráneas - cada misión pertenece a un solo astronauta 
        public int AstronautaId { get; set; }
        public int NaveId { get; set; }


        // Propiedades de navegación
        public Astronauta Astronauta { get; set; }
        public Nave Nave { get; set; }

        public ICollection<RegistroExploracion> RegistroExploracion { get; set;}
    }
}