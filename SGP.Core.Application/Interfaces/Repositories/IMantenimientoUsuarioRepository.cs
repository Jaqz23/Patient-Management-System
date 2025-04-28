using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IMantenimientoUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<List<Usuario>> GetUsuariosByConsultorioAsync(int consultorioId);
        Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> ExistsByCorreoAsync(string correo);
    
    }
}
