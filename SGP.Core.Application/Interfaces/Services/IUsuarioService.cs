using SGP.Core.Application.ViewModels.Usuario;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IUsuarioService : IGenericService<SaveUsuarioViewModel, UsuarioViewModel>
    {
        Task<UsuarioViewModel> Login(LoginViewModel loginVm);
        Task<bool> ExistsByNombreUsuario(string nombreUsuario);
        Task<bool> ExistsByCorreo(string correo);
        
    }
}
