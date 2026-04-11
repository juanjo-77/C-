using exploracion_espacial.Data;
using exploracion_espacial.Models;

namespace exploracion_espacial.Services
{
    public class MisionService
    {
        private readonly AppDbContext _context;

        public MisionService(AppDbContext context)
        {
            _context = context;
        }

        // CREAR
        public string Crear(string nombreMision, DateTime fechaLanzamiento, string estado, int astronautaId, int naveId)
        {
            string[] estadosValidos = { "planificada", "en curso", "completada", "fallida" };
            if (!estadosValidos.Contains(estado.ToLower()))
                return "Error: Estado inválido. Usa: planificada, en curso, completada o fallida.";

            var astronauta = _context.Astronautas.Find(astronautaId);
            if (astronauta == null)
                return $"Error: No existe un astronauta con Id {astronautaId}.";

            var nave = _context.Naves.Find(naveId);
            if (nave == null)
                return $"Error: No existe una nave con Id {naveId}.";

            var mision = new Mision
            {
                NombreMision = nombreMision,
                FechaLanzamiento = fechaLanzamiento,
                Estado = estado.ToLower(),
                AstronautaId = astronautaId,
                NaveId = naveId
            };

            _context.Misiones.Add(mision);
            _context.SaveChanges();
            return "Misión creada correctamente.";
        }

        // LEER TODOS
        public List<Mision> ObtenerTodos()
        {
            return _context.Misiones.ToList();
        }

        // LEER UNO
        public Mision? ObtenerPorId(int id)
        {
            return _context.Misiones.Find(id);
        }

        // ACTUALIZAR
        public string Actualizar(int id, string nombreMision, DateTime fechaLanzamiento, string estado, int astronautaId, int naveId)
        {
            var mision = _context.Misiones.Find(id);

            if (mision == null)
                return "Error: Misión no encontrada.";

            string[] estadosValidos = { "planificada", "en curso", "completada", "fallida" };
            if (!estadosValidos.Contains(estado.ToLower()))
                return "Error: Estado inválido. Usa: planificada, en curso, completada o fallida.";

            var astronauta = _context.Astronautas.Find(astronautaId);
            if (astronauta == null)
                return $"Error: No existe un astronauta con Id {astronautaId}.";

            var nave = _context.Naves.Find(naveId);
            if (nave == null)
                return $"Error: No existe una nave con Id {naveId}.";

            mision.NombreMision = nombreMision;
            mision.FechaLanzamiento = fechaLanzamiento;
            mision.Estado = estado.ToLower();
            mision.AstronautaId = astronautaId;
            mision.NaveId = naveId;

            _context.SaveChanges();
            return "Misión actualizada correctamente.";
        }

        // ELIMINAR
        public string Eliminar(int id)
        {
            var mision = _context.Misiones.Find(id);

            if (mision == null)
                return "Error: Misión no encontrada.";

            _context.Misiones.Remove(mision);
            _context.SaveChanges();
            return "Misión eliminada correctamente.";
        }
    }
}