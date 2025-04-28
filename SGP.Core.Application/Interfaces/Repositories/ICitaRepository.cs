using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface ICitaRepository : IGenericRepository<Cita>
    {
        Task AsignarPruebasALaCitaAsync(int citaId, List<int> pruebaIds);
        Task<List<Cita>> GetCitasByConsultorioAsync(int consultorioId);
        Task<List<Cita>> GetCitasByEstadoAsync(int consultorioId, EstadoCita estado);
    }
}
