using SGP.Core.Application.ViewModels.Paciente;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IPacienteService : IGenericService<SavePacienteViewModel, PacienteViewModel>
    {
        Task<List<PacienteViewModel>> GetPacientesByConsultorioAsync(int consultorioId);
        Task<bool> ExistsByCedulaAsync(string cedula);
        Task<bool> ExistsByTelefonoAsync(string telefono);
    }
}
