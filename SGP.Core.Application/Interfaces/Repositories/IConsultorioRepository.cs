using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Interfaces.Repositories
{
    public interface IConsultorioRepository : IGenericRepository<Consultorio>
    {
        Task<Consultorio> GetByNombre(string nombre);
    }
}
