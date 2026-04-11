using exploracion_espacial.Data;
using exploracion_espacial.Models;

namespace exploracion_espacial.Services
{
    public class NaveService
    {
        private readonly AppDbContext _context;

        public NaveService(AppDbContext context)
        {
            _context = context;
        }

//===========================================================================================================================

        // CREAR
        public string Crear(string nombre, string modelo, int capacidadTripulacion, string estado)
        {
            if (capacidadTripulacion <= 0)
                return "Error: La capacidad de tripulación debe ser mayor a 0.";

            string[] estadosValidos = { "operativa", "en mantenimiento", "retirada" };
            if (!estadosValidos.Contains(estado.ToLower()))
                return "Error: El estado debe ser operativa, en mantenimiento o retirada.";

            var nave = new Nave
            {
                Nombre = nombre,
                Modelo = modelo,
                CapacidadTripulacion = capacidadTripulacion,
                Estado = estado.ToLower()
            };

            _context.Naves.Add(nave);
            _context.SaveChanges();
            return "Nave creada correctamente.";
        }

//===========================================================================================================================

        // LEER TODOS
        public List<Nave> ObtenerTodos()
        {
            return _context.Naves.ToList();
        }
        
//===========================================================================================================================

        // LEER UNO
        public Nave? ObtenerPorId(int id)
        {
            return _context.Naves.Find(id);
        }

//===========================================================================================================================

        // ACTUALIZAR
        public string Actualizar(int id, string nombre, string modelo, int capacidadTripulacion, string estado)
        {
            var nave = _context.Naves.Find(id);

            if (nave == null)
                return "Error: Nave no encontrada.";

            if (capacidadTripulacion <= 0)
                return "Error: La capacidad de tripulación debe ser mayor a 0.";

            string[] estadosValidos = { "operativa", "en mantenimiento", "retirada" };
            if (!estadosValidos.Contains(estado.ToLower()))
                return "Error: El estado debe ser operativa, en mantenimiento o retirada.";

            nave.Nombre = nombre;
            nave.Modelo = modelo;
            nave.CapacidadTripulacion = capacidadTripulacion;
            nave.Estado = estado.ToLower();

            _context.SaveChanges();
            return "Nave actualizada correctamente.";
        }

//===========================================================================================================================

        // ELIMINAR
        public string Eliminar(int id)
        {
            var nave = _context.Naves.Find(id);

            if (nave == null)
                return "Error: Nave no encontrada.";

            _context.Naves.Remove(nave);
            _context.SaveChanges();
            return "Nave eliminada correctamente.";
        }
    }
}