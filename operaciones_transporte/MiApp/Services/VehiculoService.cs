using MiApp.Models;
using MiApp.Repositories;

namespace MiApp.Services;

public class VehiculoService
{
    private VehiculoRepository _repo;

    public VehiculoService(VehiculoRepository repo)
    {
        _repo = repo;
    }

    public string Registrar(string placa, int tipoInt, int capacidad)
    {
        if (placa == "")
            return "ERROR: La placa es obligatoria.";
        if (capacidad <= 0)
            return "ERROR: La capacidad debe ser mayor a cero.";
        if (tipoInt < 0 || tipoInt > 3)
            return "ERROR: Tipo de vehiculo inválido.";

        TipoVehiculo tipo = (TipoVehiculo)tipoInt;
        Vehiculos nuevo = new Vehiculos(placa.ToUpper(), tipo, capacidad);
        bool agregado = _repo.Agregar(nuevo);

        if (agregado) return "Vehiculo registrado correctamente.";
        else          return "ERROR: Ya existe un vehiculo con esa placa.";
    }

    public List<Vehiculos> ObtenerTodos()         => _repo.ObtenerTodos();
    public List<Vehiculos> ObtenerDisponibles()   => _repo.ObtenerDisponibles();
    public Vehiculos?      BuscarPorPlaca(string p) => _repo.BuscarPorPlaca(p);
}