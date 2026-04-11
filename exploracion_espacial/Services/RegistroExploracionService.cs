using exploracion_espacial.Data;
using exploracion_espacial.Models;

namespace exploracion_espacial.Services
{
    public class RegistroExploracionService
    {
        private readonly AppDbContext _context;

        public RegistroExploracionService(AppDbContext context)
        {
            _context = context;
        }

        // CREAR
        public string Crear(string planetaDestino, string descripcion, string nivelRiesgo, int misionId)
        {
            string[] nivelesValidos = { "bajo", "medio", "alto" };
            if (!nivelesValidos.Contains(nivelRiesgo.ToLower()))
                return "Error: El nivel de riesgo debe ser bajo, medio o alto.";

            var mision = _context.Misiones.Find(misionId);
            if (mision == null)
                return $"Error: No existe una misión con Id {misionId}.";

            var registro = new RegistroExploracion
            {
                PlanetaDestino = planetaDestino,
                Descripcion = descripcion,
                NivelRiesgo = nivelRiesgo.ToLower(),
                MisionId = misionId
            };

            _context.RegistrosExploracion.Add(registro);
            _context.SaveChanges();
            return "Registro de exploración creado correctamente.";
        }

        // LEER TODOS
        public List<RegistroExploracion> ObtenerTodos()
        {
            return _context.RegistrosExploracion.ToList();
        }

        // LEER UNO
        public RegistroExploracion? ObtenerPorId(int id)
        {
            return _context.RegistrosExploracion.Find(id);
        }

        // ACTUALIZAR
        public string Actualizar(int id, string planetaDestino, string descripcion, string nivelRiesgo, int misionId)
        {
            var registro = _context.RegistrosExploracion.Find(id);

            if (registro == null)
                return "Error: Registro de exploración no encontrado.";

            string[] nivelesValidos = { "bajo", "medio", "alto" };
            if (!nivelesValidos.Contains(nivelRiesgo.ToLower()))
                return "Error: El nivel de riesgo debe ser bajo, medio o alto.";

            var mision = _context.Misiones.Find(misionId);
            if (mision == null)
                return $"Error: No existe una misión con Id {misionId}.";

            registro.PlanetaDestino = planetaDestino;
            registro.Descripcion = descripcion;
            registro.NivelRiesgo = nivelRiesgo.ToLower();
            registro.MisionId = misionId;

            _context.SaveChanges();
            return "Registro de exploración actualizado correctamente.";
        }

        // ELIMINAR
        public string Eliminar(int id)
        {
            var registro = _context.RegistrosExploracion.Find(id);

            if (registro == null)
                return "Error: Registro de exploración no encontrado.";

            _context.RegistrosExploracion.Remove(registro);
            _context.SaveChanges();
            return "Registro de exploración eliminado correctamente.";
        }
    }
}