using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IResultadoLaboratorioRepository : IGenericRepository<ResultadoLaboratorio>
    {
        Task<List<ResultadoLaboratorio>> GetResultadosByCitaAsync(int citaId);
        Task<List<ResultadoLaboratorio>> GetResultadosPendientesByConsultorioAsync(int consultorioId);
        Task<List<ResultadoLaboratorio>> GetResultadosByCedulaAsync(int consultorioId, string cedula);
        Task<List<ResultadoLaboratorio>> GetResultadosCompletadosByCitaAsync(int citaId);
        Task<bool> ExisteResultadoParaCita(int citaId, int pruebaLaboratorioId);
    }
}
