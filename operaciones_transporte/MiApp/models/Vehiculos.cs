namespace MiApp.Models
{
    public class Vehiculos
    {
        public string Placa { get; set; }
        public TipoVehiculo Tipo { get; set; }
        public int Capacidad { get; set; }
        public EstadoVehiculo Estado { get; set; }
    
        public Vehiculos(string placa, TipoVehiculo tipo, int capacidad)
        {
            Placa = placa;
            Tipo = tipo;
            Capacidad = capacidad;
            Estado = EstadoVehiculo.Disponible;
        }

        public override string ToString()
        {
            return $"[{Placa}] {Tipo} | Capacidad: {Capacidad} | Estado: {Estado}";
        }

    }

}