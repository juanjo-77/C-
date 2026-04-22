
using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public class Paciente
    {
        // EF Core usa [Key] para identificar la clave primaria
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreMascota { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Especie { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NombreDueno { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Diagnostico { get; set; } = "Pendiente";

        public bool Registrado { get; set; } = false;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public Paciente() { }

        public Paciente(string nombreMascota, string especie, string nombreDueno)
        {
            NombreMascota = nombreMascota;
            Especie       = especie;
            NombreDueno   = nombreDueno;
        }

        public override string ToString()
        {
            return $"[{Id}] {NombreMascota} ({Especie}) — Dueño: {NombreDueno} — {Diagnostico}";
        }
    }
}