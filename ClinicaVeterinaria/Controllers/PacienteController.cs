
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Services;

namespace ClinicaVeterinaria.Controllers
{
    public class PacienteController
    {
        private readonly ClinicaService _clinicaService;

        public PacienteController(ClinicaService clinicaService)
        {
            _clinicaService = clinicaService;
        }

        // ============================================================
        // TASK 2 — Demostrar registro asíncrono de un paciente
        // ============================================================
        public async Task DemostrarRegistroAsync()
        {
            Console.WriteLine(" TASK 2 — Registro asíncrono\n");
            Console.WriteLine("[Main] Iniciando registro — hilo principal libre mientras espera\n");

            var paciente = new Paciente("Firulais", "Perro", "Juan García");
            await _clinicaService.RegistrarPacienteAsync(paciente);

            Console.WriteLine("\n[Main] Registro terminó, continuamos");
        }

        // ============================================================
        // TASK 3 — Demostrar tareas paralelas con WhenAll
        // ============================================================
        public async Task DemostrarTareasParalelasAsync()
        {
            Console.WriteLine(" TASK 3 — Tareas paralelas con WhenAll\n");

            var paciente = new Paciente("Rex", "Perro", "Carlos López");
            await _clinicaService.RegistrarPacienteAsync(paciente);

            await _clinicaService.ProcesarPacienteCompletoAsync(paciente, "Dr. Martínez");
        }

        // ============================================================
        // TASK 4 — Demostrar WhenAll vs WhenAny
        // ============================================================
        public async Task DemostrarWhenAllVsWhenAnyAsync()
        {
            Console.WriteLine(" TASK 4 — WhenAll vs WhenAny\n");

            // ── WhenAll: registrar varios pacientes en paralelo ──
            Console.WriteLine("  [WhenAll] Registrando 3 mascotas al mismo tiempo...\n");

            var listaPacientes = new List<Paciente>
            {
                new Paciente("Mishi",  "Gato",   "Pedro Ruiz"),
                new Paciente("Toby",   "Conejo", "Laura Díaz"),
                new Paciente("Canela", "Perro",  "Sofía Mora")
            };

            await _clinicaService.RegistrarPacientesEnParaleloAsync(listaPacientes);

            Console.WriteLine("========================================= \n");

            // ── WhenAny: primer veterinario disponible ───────────
            Console.WriteLine("  [WhenAny] Buscando primer veterinario...\n");
            string veterinario = await _clinicaService.BuscarPrimerVeterinarioAsync();
            Console.WriteLine($"\n  ✓ {veterinario} atenderá al paciente");
        }

        // ============================================================
        // TASK 5 — Demostrar buenas prácticas asíncronas
        // ============================================================
        public async Task DemostrarBuenasPracticasAsync()
        {
            Console.WriteLine(" TASK 5 — Buenas prácticas\n");

            var paciente = new Paciente("Luna", "Gata", "María Pérez");
            await _clinicaService.RegistrarPacienteAsync(paciente);

            // Usar await
            string diagnostico = await _clinicaService.ObtenerDiagnosticoAsync(paciente);
            Console.WriteLine($"\n  Diagnóstico: {diagnostico}");


            Console.WriteLine("\n  Se usó await — hilo principal libre");
            Console.WriteLine("   Nombre descriptivo: ObtenerDiagnosticoAsync");
            Console.WriteLine("   Retorna Task<string> para devolver un valor");
        }

        // ============================================================
        // Mostrar todos los pacientes guardados en MySQL
        // ============================================================
        public async Task MostrarTodosLosPacientesAsync()
        {
            Console.WriteLine(" Pacientes en MySQL\n");

            var pacientes = await _clinicaService.ObtenerTodosAsync();

            if (pacientes.Count == 0)
            {
                Console.WriteLine("  No hay pacientes registrados.");
                return;
            }

            foreach (var paciente in pacientes)
            {
                Console.WriteLine($"  {paciente}");
            }
        }
        
        // Registrar un paciente con datos ingresados por consola
        public async Task RegistrarPacienteInteractivoAsync(string nombreMascota, string especie, string nombreDueno)
        {
            Console.WriteLine();
            var paciente = new Paciente(nombreMascota, especie, nombreDueno);
            await _clinicaService.RegistrarPacienteAsync(paciente);
            Console.WriteLine($"\n  ✓ Paciente registrado con Id: {paciente.Id}");
        }
        
    }
    
}