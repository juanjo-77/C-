using exploracion_espacial.Models;
using exploracion_espacial.Data;


namespace exploracion_espacial.Services
{
    public class AstronautaService
    {
                // campo privado que guarda la conexión a la base de datos
                // 'readonly' significa que solo se asigna una vez y no cambia
        private readonly AppDbContext _context;


        public AstronautaService(AppDbContext context)
        {
            _context = context;
        }
//===========================================================================================================================

        // CREAR
        public string Crear(string nombre, string apellido, string rango, int horasExperiencia)
        {
            if (horasExperiencia <= 0)
                return "Error: Las horas de experiencia deben ser mayores a 0.";

            // arreglo con los únicos valores permitidos para rango
            string[] rangosValidos = { "novato", "piloto", "comandante" };
                            // verifica si un elemento existe dentro de la lista
            if (!rangosValidos.Contains(rango.ToLower()))
                return "Error: El rango debe ser novato, piloto o comandante.";

            var astronauta = new Astronauta
            {
                Nombre = nombre,
                Apellido = apellido,
                Rango = rango.ToLower(), // M - m
                HorasExperiencia = horasExperiencia
            };

            _context.Astronautas.Add(astronauta);  // add astronautas a la DB
            _context.SaveChanges();   // guarda los cambios en la DB
            return "Astronauta creado correctamente.";
        }

//===========================================================================================================================
        // LEER TODOS

            // ToList() ejecuta SELECT * FROM Astronautas
            // y devuelve todos los registros como una lista
        public List<Astronauta> ObtenerTodos()
        {
            return _context.Astronautas.ToList();
        }

//===========================================================================================================================
        // LEER UNO

            // Find busca por llave primaria (Id)
            // el '?' en Astronauta? significa que puede devolver null
        public Astronauta? ObtenerPorId(int id)
        {
            return _context.Astronautas.Find(id);
        }

//===========================================================================================================================

        // ACTUALIZAR
        public string Actualizar(int id, string nombre, string apellido, string rango, int horasExperiencia)
        {
            var astronauta = _context.Astronautas.Find(id);

            if (astronauta == null)
                return "Error: Astronauta no encontrado.";

            if (horasExperiencia <= 0)
                return "Error: Las horas de experiencia deben ser mayores a 0.";

            string[] rangosValidos = { "novato", "piloto", "comandante" };
            if (!rangosValidos.Contains(rango.ToLower()))
                return "Error: El rango debe ser novato, piloto o comandante.";

            astronauta.Nombre = nombre;
            astronauta.Apellido = apellido;
            astronauta.Rango = rango.ToLower();
            astronauta.HorasExperiencia = horasExperiencia;

            _context.SaveChanges();
            return "Astronauta actualizado correctamente.";
        }

//===========================================================================================================================

        // ELIMINAR
        public string Eliminar(int id)
        {
            var astronauta = _context.Astronautas.Find(id);

            if (astronauta == null)
                return "Error: Astronauta no encontrado.";

            _context.Astronautas.Remove(astronauta);
            _context.SaveChanges();
            return "Astronauta eliminado correctamente.";
        }
    }
}
