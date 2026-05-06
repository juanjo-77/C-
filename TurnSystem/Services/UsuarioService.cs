using Microsoft.EntityFrameworkCore;
using TurnSystem.Web.Data;
using TurnSystem.Web.Models;
using TurnSystem.Web.Services.Interfaces;

namespace TurnSystem.Web.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscarPorDocumentoAsync(string documento)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Documento == documento);
    }

    public async Task<bool> ExisteAsync(string documento)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Documento == documento);
    }

    public async Task<Usuario> RegistrarAsync(string documento, string nombre)
    {
        var usuario = new Usuario
        {
            Documento      = documento,
            Nombre         = nombre,
            FechaRegistro  = DateTime.Now,
            PasswordHash   = ""
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}