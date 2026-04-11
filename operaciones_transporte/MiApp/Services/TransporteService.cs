using MiApp.Models;
using MiApp.Repositories;

namespace MiApp.Services;

public class TransporteService
{
    private ServicioRepository  _servicios;
    private ConductorRepository _conductores;
    private VehiculoRepository  _vehiculos;

    public TransporteService(ServicioRepository s, ConductorRepository c, VehiculoRepository v)
    {
        _servicios   = s;
        _conductores = c;
        _vehiculos   = v;
    }

    public string RegistrarServicio(string id, string origen, string destino, double distancia)
    {
        if (origen == "" || destino == "") return "ERROR: Origen y destino obligatorios.";
        if (distancia <= 0)                return "ERROR: Distancia debe ser mayor a cero.";
        if (_servicios.BuscarPorId(id) != null) return "ERROR: Ya existe ese Id.";

        _servicios.Agregar(new Servicio(id, origen, destino, distancia));
        return "Servicio registrado.";
    }

    public string AsignarRecursos(string servicioId, string conductorId, string placa)
    {
        Servicio? s = _servicios.BuscarPorId(servicioId);
        if (s == null)                             return "ERROR: Servicio no encontrado.";
        if (s.Estado != EstadoServicio.Pendiente)  return "ERROR: El servicio no está pendiente.";

        Conductor? c = _conductores.BuscarPorId(conductorId);
        if (c == null)                                  return "ERROR: Conductor no encontrado.";
        if (c.Estado != EstadoConductor.Disponible)     return "ERROR: Conductor no disponible.";

        Vehiculos? v = _vehiculos.BuscarPorPlaca(placa);
        if (v == null)                               return "ERROR: Vehiculo no encontrado.";
        if (v.Estado != EstadoVehiculo.Disponible)   return "ERROR: Vehiculo no disponible.";

        s.ConductorAsignado = c;
        s.VehiculoAsignado  = v;
        c.Estado = EstadoConductor.EnServicio;
        v.Estado = EstadoVehiculo.EnServicio;
        return "Recursos asignados.";
    }

    public string IniciarServicio(string id)
    {
        Servicio? s = _servicios.BuscarPorId(id);
        if (s == null)                            return "ERROR: Servicio no encontrado.";
        if (s.ConductorAsignado == null || s.VehiculoAsignado == null)
                                                  return "ERROR: Faltan asignar recursos.";
        if (s.Estado != EstadoServicio.Pendiente) return "ERROR: El servicio no está pendiente.";

        s.Estado = EstadoServicio.EnCurso;
        return "Servicio iniciado.";
    }

    public string FinalizarServicio(string id)
    {
        Servicio? s = _servicios.BuscarPorId(id);
        if (s == null)                           return "ERROR: Servicio no encontrado.";
        if (s.Estado != EstadoServicio.EnCurso)  return "ERROR: El servicio no está en curso.";

        decimal tarifa = s.VehiculoAsignado!.Tipo switch
        {
            TipoVehiculo.Moto   => 1500m,
            TipoVehiculo.Carro  => 2500m,
            TipoVehiculo.Bus    => 5000m,
            TipoVehiculo.Camion => 7000m,
            _                   => 2500m    //cualquier otro es 2500
        };

        s.CostoTotal = (decimal)s.DistanciaKm * tarifa;
        s.ConductorAsignado!.Estado = EstadoConductor.Disponible;
        s.VehiculoAsignado!.Estado  = EstadoVehiculo.Disponible;
        s.Estado = EstadoServicio.Finalizado;
        return $"Servicio finalizado. Costo: ${s.CostoTotal}";
    }

    public List<Servicio> ObtenerTodos()       => _servicios.ObtenerTodos();
    public List<Servicio> ObtenerEnCurso()     => _servicios.ObtenerEnCurso();
    public List<Servicio> ObtenerFinalizados() => _servicios.ObtenerFinalizados();
}