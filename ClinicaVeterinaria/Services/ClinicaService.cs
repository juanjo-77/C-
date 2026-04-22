using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Services
{
    public class ClinicaService
    {
        private readonly AppDbContext _context;

        public ClinicaService(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // Registro asíncrono de un paciente
        // ============================================================
        public async Task RegistrarPacienteAsync(Paciente paciente)
        {
            Console.WriteLine($" Registrando a {paciente.NombreMascota}...");

            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            paciente.Registrado = true;
            Console.WriteLine($"  {paciente.NombreMascota} guardado en MySQL con Id: {paciente.Id}");
        }

        // ============================================================
        // Cargar historial clínico en segundo plano
        // ============================================================
        public async Task CargarHistorialClinicoAsync(Paciente paciente)
        {
            Console.WriteLine($"  [Historial] Cargando historial de {paciente.NombreMascota}...");
            await Task.Delay(2000);
            Console.WriteLine($"  [Historial] ✓ Historial de {paciente.NombreMascota} cargado");
        }

        // ============================================================
        // Agendar cita con el veterinario
        // ============================================================
        public async Task AgendarCitaAsync(Paciente paciente, string nombreVeterinario)
        {
            Console.WriteLine($"  [Cita] Agendando cita de {paciente.NombreMascota} con {nombreVeterinario}...");
            await Task.Delay(3000);
            Console.WriteLine($"  [Cita] ✓ Cita agendada con {nombreVeterinario}");
        }

        // ============================================================
        //  Enviar notificación al dueño
        // ============================================================
        public async Task EnviarNotificacionAsync(Paciente paciente)
        {
            Console.WriteLine($"  [Notif] Enviando notificación a {paciente.NombreDueno}...");
            await Task.Delay(1000);
            Console.WriteLine($"  [Notif] Notificación enviada a {paciente.NombreDueno}");
        }

        // ============================================================
        // Ejecutar 3 procesos en paralelo con WhenAll
        // Sin paralelismo 6 segundos
        // Con WhenAll: ~3 segundos (el más largo)
        // ============================================================
        public async Task ProcesarPacienteCompletoAsync(Paciente paciente, string veterinario)
        {
            Console.WriteLine($"\n  Procesando a {paciente.NombreMascota} en paralelo...\n");

            var inicio = DateTime.Now;

            Task tareaHistorial    = CargarHistorialClinicoAsync(paciente);
            Task tareaCita         = AgendarCitaAsync(paciente, veterinario);
            Task tareaNotificacion = EnviarNotificacionAsync(paciente);

            await Task.WhenAll(tareaHistorial, tareaCita, tareaNotificacion);

            var duracion = (DateTime.Now - inicio).Seconds;
            Console.WriteLine($"\n   Proceso completo en ~{duracion} segundos");
            Console.WriteLine("  (En serie habrían tardado ~6 segundos)");
        }

        // ============================================================
        // Registrar varios pacientes al mismo tiempo
        // Cada paciente se guarda en MySQL en paralelo
        // ============================================================
        public async Task RegistrarPacientesEnParaleloAsync(List<Paciente> pacientes)
        {
            Console.WriteLine($"  Registrando {pacientes.Count} pacientes en paralelo...\n");

            var tareas = new List<Task>();
            foreach (var paciente in pacientes)
            {
                var tareaRegistro = Task.Run(async () =>
                {
                    await using var nuevoContexto = new AppDbContext();
                    Console.WriteLine($"  Registrando a {paciente.NombreMascota}...");
                    await nuevoContexto.Pacientes.AddAsync(paciente);
                    await nuevoContexto.SaveChangesAsync();
                    paciente.Registrado = true;
                    Console.WriteLine($" {paciente.NombreMascota} guardado en MySQL con Id: {paciente.Id}");
                });
                
                tareas.Add(tareaRegistro);
            }

            await Task.WhenAll(tareas);
            Console.WriteLine("\n  Todos los pacientes registrados en MySQL");
        }

        // ============================================================
        // Buscar primer veterinario disponible con WhenAny
        // ============================================================
        public async Task<string> BuscarPrimerVeterinarioAsync()
        {
            Console.WriteLine("  Buscando veterinario disponible...\n");

            Task<string> doctor1 = VerificarDisponibilidadAsync("Dr. García",      3000);
            Task<string> doctor2 = VerificarDisponibilidadAsync("Dra. Rodríguez",  1000);
            Task<string> doctor3 = VerificarDisponibilidadAsync("Dr. Pérez",       2000);

            Task<string> primerDisponible = await Task.WhenAny(doctor1, doctor2, doctor3);
            string veterinario = await primerDisponible;
            return veterinario;
        }

        private async Task<string> VerificarDisponibilidadAsync(string nombreDoctor, int tiempoMs)
        {
            Console.WriteLine($"  [Buscar] Verificando {nombreDoctor}...");
            await Task.Delay(tiempoMs);
            Console.WriteLine($"  [Buscar] - {nombreDoctor} disponible");
            return nombreDoctor;
        }

        // ============================================================
        // Obtener diagnóstico de forma asíncrona
        // ============================================================
        public async Task<string> ObtenerDiagnosticoAsync(Paciente paciente)
        {
            string resultadoDiagnostico;
            int tiempoConsulta = 1500;

            Console.WriteLine($"  [Diagnóstico] Analizando a {paciente.NombreMascota}...");
            await Task.Delay(tiempoConsulta);

            resultadoDiagnostico = "Saludable — vacunas al día";
            paciente.Diagnostico = resultadoDiagnostico;

            // Actualiza el diagnóstico en MySQL
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();

            return resultadoDiagnostico;
        }

        // ============================================================
        // Obtener todos los pacientes de MySQL
        // ============================================================
        public async Task<List<Paciente>> ObtenerTodosAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }
    }
}