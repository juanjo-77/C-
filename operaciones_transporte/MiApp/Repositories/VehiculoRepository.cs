using MiApp.Models;

namespace MiApp.Repositories;

public class VehiculoRepository
{
    private List<Vehiculos> _lista = new List<Vehiculos>();

    public bool Agregar(Vehiculos v)
    {
        foreach (Vehiculos x in _lista)
            if (x.Placa == v.Placa) return false;

        _lista.Add(v);
        return true;
    }

    public Vehiculos? BuscarPorPlaca(string placa)
    {
        foreach (Vehiculos v in _lista)
            if (v.Placa == placa) return v;
        return null;
    }

    public List<Vehiculos> ObtenerTodos()
    {
        return _lista;
    }

    public List<Vehiculos> ObtenerDisponibles()
    {
        List<Vehiculos> resultado = new List<Vehiculos>();
        foreach (Vehiculos v in _lista)
            if (v.Estado == EstadoVehiculo.Disponible)
                resultado.Add(v);
        return resultado;
    }
}