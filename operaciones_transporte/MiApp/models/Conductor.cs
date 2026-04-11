namespace MiApp.Models;

public class Conductor
{
    public string Id       { get; set; }
    public string Nombre   { get; set; }
    public string Licencia { get; set; }
    public EstadoConductor Estado { get; set; }

    public Conductor(string id, string nombre, string licencia)
    {
        Id       = id;
        Nombre   = nombre;
        Licencia = licencia;
        Estado   = EstadoConductor.Disponible;
    }

    public override string ToString()
    {
        return $"[{Id}] {Nombre} | Licencia: {Licencia} | Estado: {Estado}";
    }
}