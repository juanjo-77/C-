using exploracion_espacial.Data;
using exploracion_espacial.Services;

var context = new AppDbContext();
context.Database.EnsureCreated();

var astronautaService = new AstronautaService(context);
var ingenieroService  = new IngenieroService(context);
var naveService       = new NaveService(context);
var misionService     = new MisionService(context);
var registroService   = new RegistroExploracionService(context);
var consultas         = new ConsultasService(context);

// ══════════════════════════════
// FUNCIONES AUXILIARES
// ══════════════════════════════

string Pedir(string mensaje)
{
    Console.Write(mensaje + ": ");
    return Console.ReadLine();
}

int PedirNumero(string mensaje)
{
    Console.Write(mensaje + ": ");
    int.TryParse(Console.ReadLine(), out int valor);
    return valor;
}

void Pausar()
{
    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
    Console.ReadKey();
}

void Titulo(string texto)
{
    Console.Clear();
    Console.WriteLine($"=== {texto} ===\n");
}

// ══════════════════════════════
// MENÚ PRINCIPAL
// ══════════════════════════════

while (true)
{
    Titulo("ASTRONOVA MISSION CONTROL");
    Console.WriteLine("1. Astronautas");
    Console.WriteLine("2. Ingenieros");
    Console.WriteLine("3. Naves");
    Console.WriteLine("4. Misiones");
    Console.WriteLine("5. Registros de Exploración");
    Console.WriteLine("6. Consultas");
    Console.WriteLine("0. Salir");
    Console.Write("\nOpción: ");

    switch (Console.ReadLine())
    {
        case "1": MenuAstronautas(); break;
        case "2": MenuIngenieros();  break;
        case "3": MenuNaves();       break;
        case "4": MenuMisiones();    break;
        case "5": MenuRegistros();   break;
        case "6": MenuConsultas();   break;
        case "0": return;
    }
}



//=======================================================================================================================


// ══════════════════════════════
// ASTRONAUTAS
// ══════════════════════════════

void MenuAstronautas()
{

    while (true)
    {
        
        Titulo("ASTRONAUTAS");
    Console.WriteLine("1. Crear  2. Listar  3. Actualizar  4. Eliminar  0. Volver");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            Console.WriteLine(astronautaService.Crear(
                Pedir("Nombre"),
                Pedir("Apellido"),
                Pedir("Rango (novato/piloto/comandante)"),
                PedirNumero("Horas de experiencia")
            ));
            break;

        case "2":
            var lista = astronautaService.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("No hay astronautas."); break; }
            foreach (var a in lista)
                Console.WriteLine($"[{a.Id}] {a.Nombre} {a.Apellido} - {a.Rango} - {a.HorasExperiencia}h");
            break;

        case "3":
            Console.WriteLine(astronautaService.Actualizar(
                PedirNumero("Id a actualizar"),
                Pedir("Nuevo nombre"),
                Pedir("Nuevo apellido"),
                Pedir("Nuevo rango (novato/piloto/comandante)"),
                PedirNumero("Nuevas horas")
            ));
            break;

        case "4":
            Console.WriteLine(astronautaService.Eliminar(PedirNumero("Id a eliminar")));
            break;
    }
    Pausar();

    }

    
}

// ══════════════════════════════
// INGENIEROS
// ══════════════════════════════

void MenuIngenieros()
{

    while (true)
    {
        
    Titulo("INGENIEROS");
    Console.WriteLine("1. Crear  2. Listar  3. Actualizar  4. Eliminar  0. Volver");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            Console.WriteLine(ingenieroService.Crear(
                Pedir("Nombre"),
                Pedir("Apellido"),
                Pedir("Especialidad"),
                PedirNumero("Años de experiencia")
            ));
            break;

        case "2":
            var lista = ingenieroService.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("No hay ingenieros."); break; }
            foreach (var i in lista)
                Console.WriteLine($"[{i.Id}] {i.Nombre} {i.Apellido} - {i.Especialidad} - {i.AniosExperiencia} años");
            break;

        case "3":
            Console.WriteLine(ingenieroService.Actualizar(
                PedirNumero("Id a actualizar"),
                Pedir("Nuevo nombre"),
                Pedir("Nuevo apellido"),
                Pedir("Nueva especialidad"),
                PedirNumero("Nuevos años")
            ));
            break;

        case "4":
            Console.WriteLine(ingenieroService.Eliminar(PedirNumero("Id a eliminar")));
            break;
    }
    Pausar();

    }

    
}

// ══════════════════════════════
// NAVES
// ══════════════════════════════

void MenuNaves()
{

    while (true)
    {

    Titulo("NAVES");
    Console.WriteLine("1. Crear  2. Listar  3. Actualizar  4. Eliminar  0. Volver");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            Console.WriteLine(naveService.Crear(
                Pedir("Nombre"),
                Pedir("Modelo"),
                PedirNumero("Capacidad de tripulación"),
                Pedir("Estado (operativa/en mantenimiento/retirada)")
            ));
            break;

        case "2":
            var lista = naveService.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("No hay naves."); break; }
            foreach (var n in lista)
                Console.WriteLine($"[{n.Id}] {n.Nombre} - {n.Modelo} - Cap:{n.CapacidadTripulacion} - {n.Estado}");
            break;

        case "3":
            Console.WriteLine(naveService.Actualizar(
                PedirNumero("Id a actualizar"),
                Pedir("Nuevo nombre"),
                Pedir("Nuevo modelo"),
                PedirNumero("Nueva capacidad"),
                Pedir("Nuevo estado")
            ));
            break;

        case "4":
            Console.WriteLine(naveService.Eliminar(PedirNumero("Id a eliminar")));
            break;
    }
    Pausar();

    }

}

// ══════════════════════════════
// MISIONES
// ══════════════════════════════

void MenuMisiones()
{

    while (true)
    {
        
    Titulo("MISIONES");
    Console.WriteLine("1. Crear  2. Listar  3. Actualizar  4. Eliminar  0. Volver");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            var fechaTexto = Pedir("Fecha de lanzamiento (dd/MM/yyyy)");
            DateTime.TryParseExact(fechaTexto, "dd/MM/yyyy",
                null, System.Globalization.DateTimeStyles.None, out DateTime fecha);
            Console.WriteLine(misionService.Crear(
                Pedir("Nombre de la misión"),
                fecha,
                Pedir("Estado (planificada/en curso/completada/fallida)"),
                PedirNumero("Id del astronauta"),
                PedirNumero("Id de la nave")
            ));
            break;

        case "2":
            var lista = misionService.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("No hay misiones."); break; }
            foreach (var m in lista)
                Console.WriteLine($"[{m.Id}] {m.NombreMision} - {m.Estado} - {m.FechaLanzamiento:dd/MM/yyyy}");
            break;

        case "3":
            var nuevaFechaTexto = Pedir("Nueva fecha (dd/MM/yyyy)");
            DateTime.TryParseExact(nuevaFechaTexto, "dd/MM/yyyy",
                null, System.Globalization.DateTimeStyles.None, out DateTime nuevaFecha);
            Console.WriteLine(misionService.Actualizar(
                PedirNumero("Id a actualizar"),
                Pedir("Nuevo nombre"),
                nuevaFecha,
                Pedir("Nuevo estado"),
                PedirNumero("Nuevo Id astronauta"),
                PedirNumero("Nuevo Id nave")
            ));
            break;

        case "4":
            Console.WriteLine(misionService.Eliminar(PedirNumero("Id a eliminar")));
            break;
    }
    Pausar();

    }

    
}

// ══════════════════════════════
// REGISTROS
// ══════════════════════════════

void MenuRegistros()
{

    while (true)
    {
        
    Titulo("REGISTROS DE EXPLORACIÓN");
    Console.WriteLine("1. Crear  2. Listar  3. Actualizar  4. Eliminar  0. Volver");
    Console.Write("Opción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            Console.WriteLine(registroService.Crear(
                Pedir("Planeta destino"),
                Pedir("Descripción"),
                Pedir("Nivel de riesgo (bajo/medio/alto)"),
                PedirNumero("Id de la misión")
            ));
            break;

        case "2":
            var lista = registroService.ObtenerTodos();
            if (!lista.Any()) { Console.WriteLine("No hay registros."); break; }
            foreach (var r in lista)
                Console.WriteLine($"[{r.Id}] {r.PlanetaDestino} - {r.NivelRiesgo} - MisiónId:{r.MisionId}");
            break;

        case "3":
            Console.WriteLine(registroService.Actualizar(
                PedirNumero("Id a actualizar"),
                Pedir("Nuevo planeta"),
                Pedir("Nueva descripción"),
                Pedir("Nuevo nivel de riesgo"),
                PedirNumero("Nuevo Id de misión")
            ));
            break;

        case "4":
            Console.WriteLine(registroService.Eliminar(PedirNumero("Id a eliminar")));
            break;
    }
    Pausar();

    }

    
}

// ══════════════════════════════
// CONSULTAS
// ══════════════════════════════

void MenuConsultas()
{

    while (true)
    {
        
    Titulo("CONSULTAS");
    Console.WriteLine("1. Todas las misiones        7. Misiones por astronauta");
    Console.WriteLine("2. Naves operativas          8. Astronautas con +3 misiones");
    Console.WriteLine("3. Astronautas por rango     9. Naves no utilizadas");
    Console.WriteLine("4. Misiones con detalle      10. Misiones con riesgo alto");
    Console.WriteLine("5. Registros por misión      11. Misiones en curso");
    Console.WriteLine("6. Misiones por estado       0. Volver");
    Console.Write("\nOpción: ");
    var opcion = Console.ReadLine();

    if (opcion == "0") return;

    switch (opcion)
    {
        case "1":
            foreach (var m in consultas.ListarTodasLasMisiones())
                Console.WriteLine($"[{m.Id}] {m.NombreMision} - {m.Estado}");
            break;
        case "2":
            foreach (var n in consultas.BuscarNavesOperativas())
                Console.WriteLine($"[{n.Id}] {n.Nombre}");
            break;
        case "3":
            consultas.FiltrarAstronautasPorRango(Pedir("Rango"))
                .ForEach(a => Console.WriteLine($"[{a.Id}] {a.Nombre} {a.Apellido}"));
            break;
        case "4":  consultas.MostrarProyeccionMisiones();       break;
        case "5":  consultas.RegistrosPorMision(PedirNumero("Id de misión"))
                       .ForEach(r => Console.WriteLine($"{r.PlanetaDestino} - {r.NivelRiesgo}"));
            break;
        case "6":  consultas.AgruparMisionesPorEstado();        break;
        case "7":  consultas.ContarMisionesPorAstronauta();     break;
        case "8":  consultas.AstronautasConMasDe3Misiones();    break;
        case "9":  consultas.NavesNoUtilizadas();               break;
        case "10": consultas.MisionesConRiesgoAlto();           break;
        case "11": consultas.MisionesEnCursoConRegistros();     break;
    }
    Pausar();

    }

    
}