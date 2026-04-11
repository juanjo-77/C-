public class Tarea
{
    public string Titulo { get; set; }
    public Prioridad Prioridad { get; set; }
    public bool Completada { get; set; }

    // Constructor
    public Tarea(string titulo, Prioridad prioridad)
    {
        Titulo = titulo;
        Prioridad = prioridad;
        Completada = false;
    }

    // Sobrescribimos el método ToString para imprimir la tarea de manera legible
    public override string ToString()
    {
        return $"Tarea [Título: {Titulo}, Prioridad: {Prioridad}, Completada: {Completada}]";
    }
}