using MiApp.Models;

namespace MiApp.Repositories;

public class ConductorRepository
{
    private List<Conductor> _lista = new List<Conductor>();

    //evitar duplicados, si ya existe devuelve false
    public bool Agregar(Conductor c)
    {
        foreach (Conductor x in _lista)
            if (x.Id == c.Id) return false;  

        _lista.Add(c);
        return true;
    }

    //busca por id, si no encuentra devuelve null
    public Conductor? BuscarPorId(string id)
    {
        foreach (Conductor c in _lista)
            if (c.Id == id) return c;
        return null;
    }

    //devuelve todos los conductores registrados
    public List<Conductor> ObtenerTodos()
    {
        return _lista;
    }

     // devuelve solo los conductores disponibles
    public List<Conductor> ObtenerDisponibles()
    {
        List<Conductor> resultado = new List<Conductor>();
        foreach (Conductor c in _lista)
            if (c.Estado == EstadoConductor.Disponible)
                resultado.Add(c);  // si está disponible, lo agrega a la lista resultado
        return resultado;
    }
}