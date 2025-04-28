using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> LoginAsync(LoginViewModel loginVm);
        Task<bool> ExistsByNombreUsuario(string nombreUsuario);
        Task<bool> ExistsByCorreo(string correo);

    }
}
