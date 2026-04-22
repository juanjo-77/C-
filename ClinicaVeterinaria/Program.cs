using System;
using System.Threading.Tasks;
using ClinicaVeterinaria.Controllers;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Services;

class Program
{
    static async Task Main(string[] args)
    {
        await using var context = new AppDbContext();
        var clinicaService      = new ClinicaService(context);
        var controller          = new PacienteController(clinicaService);

        bool salir = false;

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║     CLÍNICA VETERINARIA — ASYNC MVC      ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");

        while (!salir)
        {
            MostrarMenu();

            string opcion = Console.ReadLine()?.Trim() ?? "";

            switch (opcion)
            {
                case "1":
                    await RegistrarPacienteInteractivoAsync(controller);
                    break;

                case "2":
                    await controller.MostrarTodosLosPacientesAsync();
                    break;

                case "3":
                    await controller.DemostrarTareasParalelasAsync();
                    break;

                case "4":
                    await controller.DemostrarWhenAllVsWhenAnyAsync();
                    break;

                case "5":
                    await controller.DemostrarBuenasPracticasAsync();
                    break;

                case "6":
                    salir = true;
                    Console.WriteLine("\n  Hasta luego 👋");
                    break;

                default:
                    Console.WriteLine("\n  Opción inválida, intenta de nuevo.");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine("\nPresiona Enter para continuar...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    // ============================================================
    // Muestra el menú principal
    // ============================================================
    static void MostrarMenu()
    {
        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine("              MENÚ PRINCIPAL              ");
        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine("  1. Registrar nuevo paciente             ");
        Console.WriteLine("  2. Ver todos los pacientes              ");
        Console.WriteLine("  3. Demostrar tareas paralelas (WhenAll) ");
        Console.WriteLine("  4. Demostrar WhenAll vs WhenAny         ");
        Console.WriteLine("  5. Demostrar buenas prácticas async     ");
        Console.WriteLine("  6. Salir                                ");
        Console.WriteLine("══════════════════════════════════════════");
        Console.Write("\n  Elige una opción: ");
    }

    // ============================================================
    // Pide los datos del paciente por consola y lo registra
    // ============================================================
    static async Task RegistrarPacienteInteractivoAsync(PacienteController controller)
    {
        Console.WriteLine("\n── Registrar nuevo paciente ──\n");

        Console.Write("  Nombre de la mascota: ");
        string nombreMascota = Console.ReadLine()?.Trim() ?? "Sin nombre";

        Console.Write("  Especie (Perro, Gato, Conejo, etc.): ");
        string especie = Console.ReadLine()?.Trim() ?? "Desconocida";

        Console.Write("  Nombre del dueño: ");
        string nombreDueno = Console.ReadLine()?.Trim() ?? "Sin dueño";

        await controller.RegistrarPacienteInteractivoAsync(nombreMascota, especie, nombreDueno);
    }
}