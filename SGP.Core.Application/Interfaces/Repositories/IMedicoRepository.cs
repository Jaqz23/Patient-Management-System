using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IMedicoRepository : IGenericRepository<Medico>
    {
        Task<bool> ExistsByCedulaAsync(string cedula);
        Task<bool> ExistsByCorreoAsync(string correo);
        Task<bool> ExistsByTelefono(string telefono);
        Task<List<Medico>> GetMedicosByConsultorioAsync(int consultorioId);
    }
}
