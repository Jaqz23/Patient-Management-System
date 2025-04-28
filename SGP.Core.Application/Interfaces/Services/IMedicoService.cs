using SGP.Core.Application.ViewModels.Medico;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IMedicoService : IGenericService<SaveMedicoViewModel, MedicoViewModel>
    {
        Task<List<MedicoViewModel>> GetMedicosByConsultorioAsync(int consultorioId);
        Task<bool> ExistsByCedula(string cedula);
        Task<bool> ExistsByCorreo(string correo);
        Task<bool> ExistsByTelefono(string telefono);

    }
}
