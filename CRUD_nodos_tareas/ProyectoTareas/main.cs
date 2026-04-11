using System;

class Program
{
    static void Main()
    {
        // Crear el sistema de tareas
        SistemaTareas sistema = new SistemaTareas();
        
        // Menú interactivo
        int opcion;
        do
        {
            Console.WriteLine("\n--- Menú ---");
            Console.WriteLine("1. Agregar tarea");
            Console.WriteLine("2. Eliminar tarea");
            Console.WriteLine("3. Mostrar todas las tareas");
            Console.WriteLine("4. Completar tarea");
            Console.WriteLine("5. Buscar tarea por título");
            Console.WriteLine("6. Salir");
            Console.Write("Elige una opción: ");
            
            opcion = int.Parse(Console.ReadLine()); // Leer opción

            switch (opcion)
            {
                case 1:
                    // Agregar tarea
                    Console.Write("Ingresa el título de la tarea: ");
                    string tituloAgregar = Console.ReadLine();
                    Console.Write("Ingresa la prioridad (ALTA, MEDIA, BAJA): ");
                    string prioridadAgregar = Console.ReadLine().ToUpper();
                    Prioridad prioridad = (Prioridad)Enum.Parse(typeof(Prioridad), prioridadAgregar);
                    sistema.AgregarTarea(tituloAgregar, prioridad);
                    Console.WriteLine("Tarea agregada con éxito.");
                    break;

                case 2:
                    // Eliminar tarea
                    Console.Write("Ingresa el título de la tarea a eliminar: ");
                    string tituloEliminar = Console.ReadLine();
                    sistema.EliminarTarea(tituloEliminar);
                    Console.WriteLine("Tarea eliminada (si existía).");
                    break;

                case 3:
                    // Mostrar todas las tareas
                    Console.WriteLine("\nLista de tareas:");
                    sistema.MostrarTareas();
                    break;

                case 4:
                    // Completar tarea
                    Console.Write("Ingresa el título de la tarea a completar: ");
                    string tituloCompletar = Console.ReadLine();
                    sistema.CompletarTarea(tituloCompletar);
                    Console.WriteLine("Tarea completada (si existía).");
                    break;

                case 5:
                    // Buscar tarea
                    Console.Write("Ingresa el título de la tarea a buscar: ");
                    string tituloBuscar = Console.ReadLine();
                    Tarea tareaEncontrada = sistema.BuscarTarea(tituloBuscar);
                    if (tareaEncontrada != null)
                    {
                        Console.WriteLine("Tarea encontrada: " + tareaEncontrada);
                    }
                    else
                    {
                        Console.WriteLine("Tarea no encontrada.");
                    }
                    break;

                case 6:
                    // Salir
                    Console.WriteLine("¡Adiós!");
                    break;

                default:
                    Console.WriteLine("Opción no válida. Intenta de nuevo.");
                    break;
            }
        } while (opcion != 6);  // El bucle sigue hasta que el usuario elige salir
    }
}