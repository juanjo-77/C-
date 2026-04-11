using MiApp.Models;
using MiApp.Repositories;

namespace MiApp.Services;

public class ConductorService
{
    private ConductorRepository _repo;

    public ConductorService(ConductorRepository repo)
    {
        _repo = repo;
    }

    public string Registrar(string id, string nombre, string licencia)
    {
        if (id == "" || nombre == "" || licencia == "")
            return "ERROR: Todos los campos son obligatorios.";

        Conductor nuevo = new Conductor(id, nombre, licencia);
        bool agregado = _repo.Agregar(nuevo);

        if (agregado) return "Conductor registrado correctamente.";
        else          return "ERROR: Ya existe un conductor con ese Id.";
    }

    public List<Conductor> ObtenerTodos()        => _repo.ObtenerTodos();
    public List<Conductor> ObtenerDisponibles()  => _repo.ObtenerDisponibles();
    public Conductor?      BuscarPorId(string id) => _repo.BuscarPorId(id);
}