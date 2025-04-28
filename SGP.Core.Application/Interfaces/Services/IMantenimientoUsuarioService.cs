using SGP.Core.Application.ViewModels.MantenimientoUsuario;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IMantenimientoUsuarioService : IGenericService<SaveMantenimientoUsuarioViewModel, MantenimientoUsuarioViewModel>
    {
        Task<List<MantenimientoUsuarioViewModel>> GetUsuariosByConsultorioAsync(int consultorioId);
        Task<bool> ExistsByNombreUsuario(string nombreUsuario);
        Task<bool> ExistsByCorreo(string correo);
    }
}
