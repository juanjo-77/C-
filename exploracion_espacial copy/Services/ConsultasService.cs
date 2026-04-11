using exploracion_espacial.Data;
using exploracion_espacial.Models;
using Microsoft.EntityFrameworkCore;

namespace exploracion_espacial.Services
{
    public class ConsultasService
    {
        private readonly AppDbContext _context;

        public ConsultasService(AppDbContext context)
        {
            _context = context;
        }

//===========================================================================================================================

        // ============================================================
        // CONSULTAS BÁSICAS
        // ============================================================

        // Listar todas las misiones
        public List<Mision> ListarTodasLasMisiones()
        {
            return _context.Misiones.ToList();  // Trae todas las misiones
        }

        // Buscar naves operativas
        public List<Nave> BuscarNavesOperativas()
        {
            return _context.Naves
                .Where(n => n.Estado == "operativa")   //// Where filtra registros que cumplan la condición
                .ToList();
        }

        // Filtrar astronautas por rango
        public List<Astronauta> FiltrarAstronautasPorRango(string rango)
        {
            return _context.Astronautas
                .Where(a => a.Rango == rango.ToLower())
                .ToList();
        }

//===========================================================================================================================

        // ============================================================
        // CONSULTAS CON RELACIONES (Include / ThenInclude)
        // ============================================================

        // Obtener misiones con su astronauta y nave
        public List<Mision> MisionesConAstronautaYNave()
        {
            return _context.Misiones
                .Include(m => m.Astronauta)   //include (traer datos relacionados) de astronauta
                .Include(m => m.Nave)         //include (traer datos relacionados) de nave
                .ToList();
        }

        // Obtener registros de exploración por misión
        public List<RegistroExploracion> RegistrosPorMision(int misionId)
        {
            return _context.RegistrosExploracion
                .Include(r => r.Mision)
                .Where(r => r.MisionId == misionId)
                .ToList();
        }

//===========================================================================================================================

        // ============================================================
        // PROYECCIONES (Select)
        // ============================================================

        // Mostrar nombre de misión, astronauta y nave
        public void MostrarProyeccionMisiones()
        {
            var resultado = _context.Misiones
                .Include(m => m.Astronauta)
                .Include(m => m.Nave)
                .Select(m => new
                {
                    Mision = m.NombreMision,
                    Astronauta = m.Astronauta.Nombre + " " + m.Astronauta.Apellido,
                    Nave = m.Nave.Nombre,
                    Estado = m.Estado
                })
                .ToList();

            foreach (var r in resultado)
                Console.WriteLine($"  Misión: {r.Mision} | Astronauta: {r.Astronauta} | Nave: {r.Nave} | Estado: {r.Estado}");
        }

//===========================================================================================================================

        // ============================================================
        // AGRUPACIONES
        // ============================================================

        // Agrupar misiones por estado
        public void AgruparMisionesPorEstado()
        {
            var grupos = _context.Misiones
                .GroupBy(m => m.Estado) // GroupBy agrupa todos los registros que tengan el mismo valor en Estado
                .Select(g => new
                {
                    Estado = g.Key,       // el valor del estado (planificada, en curso, etc.)
                    Total = g.Count()    // cuántas misiones tienen ese estado
                })
                .ToList();

            foreach (var g in grupos)
                Console.WriteLine($"  Estado: {g.Estado} | Total misiones: {g.Total}");
        }

//===========================================================================================================================

        // Contar misiones por astronauta
        public void ContarMisionesPorAstronauta()
        {
            var resultado = _context.Misiones
                .Include(m => m.Astronauta)
                .GroupBy(m => new { m.AstronautaId, m.Astronauta.Nombre, m.Astronauta.Apellido })
                .Select(g => new
                {
                    Astronauta = g.Key.Nombre + " " + g.Key.Apellido,
                    TotalMisiones = g.Count()
                })
                .ToList();

            foreach (var r in resultado)
                Console.WriteLine($"  Astronauta: {r.Astronauta} | Misiones: {r.TotalMisiones}");
        }

//===========================================================================================================================

        // ============================================================
        // CONSULTAS AVANZADAS
        // ============================================================

        // Astronautas con más de 3 misiones
        public void AstronautasConMasDe3Misiones()
        {
            var resultado = _context.Misiones
                .Include(m => m.Astronauta)
                .GroupBy(m => new { m.AstronautaId, m.Astronauta.Nombre, m.Astronauta.Apellido })

                // Where después de GroupBy filtra los GRUPOS, no los registros
                // solo nos quedamos con grupos que tengan más de 3 misiones
                .Where(g => g.Count() > 3)
                .Select(g => new
                {
                    Astronauta = g.Key.Nombre + " " + g.Key.Apellido,
                    TotalMisiones = g.Count()
                })
                .ToList();

            // Any() devuelve true si hay al menos un elemento en la lista
            if (!resultado.Any())
            {
                Console.WriteLine("  Ningún astronauta tiene más de 3 misiones.");
                return;
            }

            foreach (var r in resultado)
                Console.WriteLine($"  {r.Astronauta} - {r.TotalMisiones} misiones");
        }

//===========================================================================================================================

        // Naves no utilizadas (sin misiones asignadas)
        public void NavesNoUtilizadas()
        {
            var resultado = _context.Naves
                .Where(n => !_context.Misiones.Any(m => m.NaveId == n.Id))
                .ToList();

            if (!resultado.Any())
            {
                Console.WriteLine("  Todas las naves tienen misiones asignadas.");
                return;
            }

            foreach (var n in resultado)
                Console.WriteLine($"  [{n.Id}] {n.Nombre} - {n.Modelo}");
        }

//===========================================================================================================================

        // Misiones con nivel de riesgo alto
        public void MisionesConRiesgoAlto()
        {
            var resultado = _context.RegistrosExploracion
                .Include(r => r.Mision)
                .Where(r => r.NivelRiesgo == "alto")
                .Select(r => new
                {
                    Mision = r.Mision.NombreMision,
                    Planeta = r.PlanetaDestino,
                    Riesgo = r.NivelRiesgo
                })
                .ToList();

            if (!resultado.Any())
            {
                Console.WriteLine("  No hay misiones con riesgo alto.");
                return;
            }

            foreach (var r in resultado)
                Console.WriteLine($"  Misión: {r.Mision} | Planeta: {r.Planeta} | Riesgo: {r.Riesgo}");
        }

//===========================================================================================================================

        // Misiones en curso con sus registros asociados
        public void MisionesEnCursoConRegistros()
        {
            var resultado = _context.Misiones
                .Include(m => m.RegistroExploracion)
                .Where(m => m.Estado == "en curso")
                .ToList();

            if (!resultado.Any())
            {
                Console.WriteLine("  No hay misiones en curso.");
                return;
            }

            foreach (var m in resultado)
            {
                Console.WriteLine($"  Misión: {m.NombreMision}");
                foreach (var r in m.RegistroExploracion)
                    Console.WriteLine($"    → {r.PlanetaDestino} | {r.NivelRiesgo} | {r.Descripcion}");
            }
        }
    }
}