namespace exploracion_espacial.Models
{
    public class Astronauta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rango { get; set; }
        public int HorasExperiencia { get; set; }

        // un astronauta puede tener varias misiones
        public ICollection<Mision> Misiones { get; set;}
    }
}