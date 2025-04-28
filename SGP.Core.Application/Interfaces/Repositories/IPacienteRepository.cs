using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IPacienteRepository : IGenericRepository<Paciente>
    {
        Task<bool> ExistsByCedulaAsync(string cedula);
        Task<bool> ExistsByTelefonoAsync(string telefono);
        Task<List<Paciente>> GetPacientesByConsultorioAsync(int consultorioId);
    }
}
