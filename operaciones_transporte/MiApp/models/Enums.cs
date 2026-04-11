namespace MiApp.Models;

public enum EstadoConductor { 
    Disponible, 
    EnServicio, 
    Inactivo 
}

public enum EstadoVehiculo  { 
    Disponible, 
    EnServicio, 
    FueraDeOperacion
}

public enum EstadoServicio  { 
    Pendiente, 
    EnCurso, 
    Finalizado 
}

public enum TipoVehiculo    { 
    Carro, 
    Moto, 
    Camion, 
    Bus
}