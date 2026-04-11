using MiApp.Models;
using MiApp.Repositories;
using MiApp.Services;

ConductorRepository repoConductores = new ConductorRepository();
VehiculoRepository  repoVehiculos   = new VehiculoRepository();
ServicioRepository  repoServicios   = new ServicioRepository();

ConductorService  svcConductor  = new ConductorService(repoConductores);
VehiculoService   svcVehiculo   = new VehiculoService(repoVehiculos);
TransporteService svcTransporte = new TransporteService(repoServicios, repoConductores, repoVehiculos);

string opcion;

do
{
    Console.Clear();
    Console.WriteLine("=== Sistema de Transporte ===");
    Console.WriteLine("1. Registrar conductor");
    Console.WriteLine("2. Registrar vehiculo");
    Console.WriteLine("3. Registrar servicio");
    Console.WriteLine("4. Asignar conductor y vehiculo");
    Console.WriteLine("5. Iniciar servicio");
    Console.WriteLine("6. Finalizar servicio");
    Console.WriteLine("7. Ver servicios");
    Console.WriteLine("8. Ver conductores y vehiculos");
    Console.WriteLine("9. Reportes");
    Console.WriteLine("0. Salir");
    Console.Write("Opcion: ");
    opcion = Console.ReadLine() ?? "";

    Console.Clear();

    switch (opcion)
    {
        case "1":
            Console.WriteLine("── Registrar Conductor ──");
            Console.Write("Id: ");       string id1      = Console.ReadLine() ?? "";
            Console.Write("Nombre: ");   string nombre   = Console.ReadLine() ?? "";
            Console.Write("Licencia: "); string licencia = Console.ReadLine() ?? "";
            Console.WriteLine(svcConductor.Registrar(id1, nombre, licencia));
            break;

        case "2":
            Console.WriteLine("── Registrar Vehiculo ──");
            Console.Write("Placa: "); string placa2 = Console.ReadLine() ?? "";

            Console.WriteLine("Tipo: 0=Carro  1=Moto  2=Camion  3=Bus");
            Console.Write("Tipo: "); string tipoStr = Console.ReadLine() ?? "";
            Console.Write("Capacidad: "); string capStr = Console.ReadLine() ?? "";

            if (!int.TryParse(tipoStr, out int tipoInt) || !int.TryParse(capStr, out int capacidad))
            {
                Console.WriteLine("ERROR: Ingresa solo numeros.");
                break;
            }

            Console.WriteLine(svcVehiculo.Registrar(placa2, tipoInt, capacidad));
            break;

        case "3":
            Console.WriteLine("── Registrar Servicio ──");
            Console.Write("Id: ");           string id3     = Console.ReadLine() ?? "";
            Console.Write("Origen: ");       string origen  = Console.ReadLine() ?? "";
            Console.Write("Destino: ");      string destino = Console.ReadLine() ?? "";
            Console.Write("Distancia km: "); double dist    = double.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine(svcTransporte.RegistrarServicio(id3, origen, destino, dist));
            break;

        case "4":
            Console.WriteLine("── Asignar Recursos ──");
            Console.WriteLine("\nConductores disponibles:");
            foreach (Conductor c in svcConductor.ObtenerDisponibles())
                Console.WriteLine("  " + c);
            Console.WriteLine("\nVehiculos disponibles:");
            foreach (Vehiculos v in svcVehiculo.ObtenerDisponibles())
                Console.WriteLine("  " + v);
            Console.Write("\nId del servicio: ");  string sId  = Console.ReadLine() ?? "";
            Console.Write("Id del conductor: ");   string cId  = Console.ReadLine() ?? "";
            Console.Write("Placa del vehiculo: "); string placa4 = Console.ReadLine() ?? "";
            Console.WriteLine(svcTransporte.AsignarRecursos(sId, cId, placa4));
            break;

        case "5":
            Console.WriteLine("── Iniciar Servicio ──");
            foreach (Servicio s in svcTransporte.ObtenerTodos())
                if (s.Estado == EstadoServicio.Pendiente)
                    Console.WriteLine("  " + s);
            Console.Write("Id del servicio: "); string id5 = Console.ReadLine() ?? "";
            Console.WriteLine(svcTransporte.IniciarServicio(id5));
            break;

        case "6":
            Console.WriteLine("── Finalizar Servicio ──");
            foreach (Servicio s in svcTransporte.ObtenerEnCurso())
                Console.WriteLine("  " + s);
            Console.Write("Id del servicio: "); string id6 = Console.ReadLine() ?? "";
            Console.WriteLine(svcTransporte.FinalizarServicio(id6));
            break;

        case "7":
            Console.WriteLine("── Todos los Servicios ──");
            List<Servicio> todos7 = svcTransporte.ObtenerTodos();
            if (todos7.Count == 0) Console.WriteLine("No hay servicios.");
            foreach (Servicio s in todos7)
                Console.WriteLine("  " + s);
            break;

        case "8":
            Console.WriteLine("── Conductores ──");
            foreach (Conductor c in svcConductor.ObtenerTodos())
                Console.WriteLine("  " + c);
            Console.WriteLine("\n── Vehiculos ──");
            foreach (Vehiculos v in svcVehiculo.ObtenerTodos())
                Console.WriteLine("  " + v);
            break;

        case "9":
            Console.WriteLine("── Reportes ──");
            decimal ingresos = 0;
            foreach (Servicio s in svcTransporte.ObtenerFinalizados())
                ingresos += s.CostoTotal;

            int conductoresOcupados = 0;
            foreach (Conductor c in svcConductor.ObtenerTodos())
                if (c.Estado == EstadoConductor.EnServicio) conductoresOcupados++;

            int vehiculosOcupados = 0;
            foreach (Vehiculos v in svcVehiculo.ObtenerTodos())
                if (v.Estado == EstadoVehiculo.EnServicio) vehiculosOcupados++;

            Console.WriteLine($"Registrados  : {svcTransporte.ObtenerTodos().Count}");
            Console.WriteLine($"En curso     : {svcTransporte.ObtenerEnCurso().Count}");
            Console.WriteLine($"Finalizados  : {svcTransporte.ObtenerFinalizados().Count}");
            Console.WriteLine($"Ingresos     : ${ingresos}");
            Console.WriteLine($"Conductores ocupados : {conductoresOcupados}");
            Console.WriteLine($"Vehiculos ocupados   : {vehiculosOcupados}");
            break;

        case "0":
            Console.WriteLine("Hasta luego.");
            break;

        default:
            Console.WriteLine("Opcion invalida.");
            break;
    }

    if (opcion != "0")
    {
        Console.WriteLine("\nPresiona Enter para continuar...");
        Console.ReadLine();
    }

} while (opcion != "0");