using exploracion_espacial.Data;
using exploracion_espacial.Services;

var context = new AppDbContext();
context.Database.EnsureCreated();

var astronautaService = new AstronautaService(context);
var ingenieroService = new IngenieroService(context);
var naveService = new NaveService(context);
var misionService = new MisionService(context);
var registroService = new RegistroExploracionService(context);
var consultas = new ConsultasService(context);

// ============ DATOS DE PRUEBA ============
Console.WriteLine("=== INSERTANDO DATOS DE PRUEBA ===");

Console.WriteLine(astronautaService.Crear("Neil", "Armstrong", "comandante", 5000));
Console.WriteLine(astronautaService.Crear("Buzz", "Aldrin", "piloto", 3000));
Console.WriteLine(astronautaService.Crear("Sally", "Ride", "novato", 1200));

Console.WriteLine(ingenieroService.Crear("Ada", "Lovelace", "IA", 10));
Console.WriteLine(ingenieroService.Crear("Alan", "Turing", "sistemas", 8));

Console.WriteLine(naveService.Crear("Apolo 13", "Lunar Module", 3, "operativa"));
Console.WriteLine(naveService.Crear("Discovery", "Space Shuttle", 7, "operativa"));
Console.WriteLine(naveService.Crear("Atlantis", "Space Shuttle", 7, "en mantenimiento"));

var a1 = astronautaService.ObtenerTodos()[0].Id;
var a2 = astronautaService.ObtenerTodos()[1].Id;
var n1 = naveService.ObtenerTodos()[0].Id;
var n2 = naveService.ObtenerTodos()[1].Id;

Console.WriteLine(misionService.Crear("Apolo Mission 1", DateTime.Now, "completada", a1, n1));
Console.WriteLine(misionService.Crear("Marte 2025", DateTime.Now.AddDays(30), "en curso", a1, n1));
Console.WriteLine(misionService.Crear("Luna Base Alpha", DateTime.Now.AddDays(60), "planificada", a2, n2));
Console.WriteLine(misionService.Crear("Deep Space 9", DateTime.Now.AddDays(90), "en curso", a1, n2));

var m1 = misionService.ObtenerTodos()[0].Id;
var m2 = misionService.ObtenerTodos()[1].Id;

Console.WriteLine(registroService.Crear("Marte", "Superficie rocosa con tormentas de polvo.", "alto", m2));
Console.WriteLine(registroService.Crear("Luna", "Zona de impacto de meteoritos.", "medio", m1));
Console.WriteLine(registroService.Crear("Júpiter", "Gran mancha roja visible.", "bajo", m2));

// ============ CONSULTAS BÁSICAS ============
Console.WriteLine("\n=== CONSULTAS BÁSICAS ===");

Console.WriteLine("\n-- Todas las misiones --");
var misiones = consultas.ListarTodasLasMisiones();
foreach (var m in misiones)
    Console.WriteLine($"  [{m.Id}] {m.NombreMision} - {m.Estado}");

Console.WriteLine("\n-- Naves operativas --");
var navesOp = consultas.BuscarNavesOperativas();
foreach (var n in navesOp)
    Console.WriteLine($"  [{n.Id}] {n.Nombre} - {n.Estado}");

Console.WriteLine("\n-- Astronautas con rango piloto --");
var pilotos = consultas.FiltrarAstronautasPorRango("piloto");
foreach (var a in pilotos)
    Console.WriteLine($"  [{a.Id}] {a.Nombre} {a.Apellido}");

// ============ CONSULTAS CON RELACIONES ============
Console.WriteLine("\n=== CONSULTAS CON RELACIONES ===");

Console.WriteLine("\n-- Misiones con astronauta y nave --");
consultas.MostrarProyeccionMisiones();

Console.WriteLine("\n-- Registros de la misión 1 --");
var registros = consultas.RegistrosPorMision(m1);
foreach (var r in registros)
    Console.WriteLine($"  {r.PlanetaDestino} - {r.NivelRiesgo} - {r.Descripcion}");

// ============ AGRUPACIONES ============
Console.WriteLine("\n=== AGRUPACIONES ===");

Console.WriteLine("\n-- Misiones agrupadas por estado --");
consultas.AgruparMisionesPorEstado();

Console.WriteLine("\n-- Misiones por astronauta --");
consultas.ContarMisionesPorAstronauta();

// ============ CONSULTAS AVANZADAS ============
Console.WriteLine("\n=== CONSULTAS AVANZADAS ===");

Console.WriteLine("\n-- Astronautas con más de 3 misiones --");
consultas.AstronautasConMasDe3Misiones();

Console.WriteLine("\n-- Naves no utilizadas --");
consultas.NavesNoUtilizadas();

Console.WriteLine("\n-- Misiones con riesgo alto --");
consultas.MisionesConRiesgoAlto();

Console.WriteLine("\n-- Misiones en curso con registros --");
consultas.MisionesEnCursoConRegistros();