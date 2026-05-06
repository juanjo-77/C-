using TurnSystem.Web.Models;

namespace TurnSystem.Web.Services.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> BuscarPorDocumentoAsync(string documento);
    Task<Usuario> RegistrarAsync(string documento, string nombre);
    Task<bool> ExisteAsync(string documento);
}