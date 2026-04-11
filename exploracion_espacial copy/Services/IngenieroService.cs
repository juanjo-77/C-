using exploracion_espacial.Data;
using exploracion_espacial.Models;

namespace exploracion_espacial.Services
{
    public class IngenieroService
    {
        private readonly AppDbContext _context;

        public IngenieroService(AppDbContext context)
        {
            _context = context;
        }

        // CREAR
        public string Crear(string nombre, string apellido, string especialidad, int aniosExperiencia)
        {
            if (aniosExperiencia < 0)
                return "Error: Los años de experiencia no pueden ser negativos.";

            var ingeniero = new Ingeniero
            {
                Nombre = nombre,
                Apellido = apellido,
                Especialidad = especialidad,
                AniosExperiencia = aniosExperiencia
            };

            _context.Ingenieros.Add(ingeniero);
            _context.SaveChanges();
            return "Ingeniero creado correctamente.";
        }

        // LEER TODOS
        public List<Ingeniero> ObtenerTodos()
        {
            return _context.Ingenieros.ToList();
        }

        // LEER UNO
        public Ingeniero? ObtenerPorId(int id)
        {
            return _context.Ingenieros.Find(id);
        }

        // ACTUALIZAR
        public string Actualizar(int id, string nombre, string apellido, string especialidad, int aniosExperiencia)
        {
            var ingeniero = _context.Ingenieros.Find(id);

            if (ingeniero == null)
                return "Error: Ingeniero no encontrado.";

            if (aniosExperiencia < 0)
                return "Error: Los años de experiencia no pueden ser negativos.";

            ingeniero.Nombre = nombre;
            ingeniero.Apellido = apellido;
            ingeniero.Especialidad = especialidad;
            ingeniero.AniosExperiencia = aniosExperiencia;

            _context.SaveChanges();
            return "Ingeniero actualizado correctamente.";
        }

        // ELIMINAR
        public string Eliminar(int id)
        {
            var ingeniero = _context.Ingenieros.Find(id);

            if (ingeniero == null)
                return "Error: Ingeniero no encontrado.";

            _context.Ingenieros.Remove(ingeniero);
            _context.SaveChanges();
            return "Ingeniero eliminado correctamente.";
        }
    }
}