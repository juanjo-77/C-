using MiApp.Models;

namespace MiApp.Repositories;

public class ServicioRepository
{
    private List<Servicio> _lista = new List<Servicio>();

    public bool Agregar(Servicio s)
    {
        foreach (Servicio x in _lista)
            if (x.Id == s.Id) return false;

        _lista.Add(s);
        return true;
    }

    public Servicio? BuscarPorId(string id)
    {
        foreach (Servicio s in _lista)
            if (s.Id == id) return s;
        return null;
    }

    public List<Servicio> ObtenerTodos()
    {
        return _lista;
    }

    public List<Servicio> ObtenerEnCurso()
    {
        List<Servicio> resultado = new List<Servicio>();
        foreach (Servicio s in _lista)
            if (s.Estado == EstadoServicio.EnCurso)
                resultado.Add(s);
        return resultado;
    }

    public List<Servicio> ObtenerFinalizados()
    {
        List<Servicio> resultado = new List<Servicio>();
        foreach (Servicio s in _lista)
            if (s.Estado == EstadoServicio.Finalizado)
                resultado.Add(s);
        return resultado;
    }
}