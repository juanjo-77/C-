namespace MiApp.Models;

public class Servicio
{
    public string Id          { get; set; }
    public string Origen      { get; set; }
    public string Destino     { get; set; }
    public double DistanciaKm { get; set; }
    public decimal CostoTotal { get; set; }
    public EstadoServicio Estado { get; set; }

    public Conductor? ConductorAsignado { get; set; }
    public Vehiculos?  VehiculoAsignado  { get; set; }

    public Servicio(string id, string origen, string destino, double distancia)
    {
        Id           = id;
        Origen       = origen;
        Destino      = destino;
        DistanciaKm  = distancia;
        CostoTotal   = 0;
        Estado       = EstadoServicio.Pendiente;
    }

    public override string ToString()
    {
        string c = ConductorAsignado?.Nombre ?? "Sin asignar";
        string v = VehiculoAsignado?.Placa   ?? "Sin asignar";
        return $"[{Id}] {Origen} -> {Destino} | {DistanciaKm}km | " +
               $"Estado: {Estado} | Costo: ${CostoTotal}\n" +
               $"       Conductor: {c} | Vehiculo: {v}";
    }
}