using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class ConsultorioRepository : GenericRepository<Consultorio>, IConsultorioRepository
    {
        private readonly ApplicationContext _dbContext;

        public ConsultorioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Consultorio> GetByNombre(string nombre)
        {
            return await _dbContext.Consultorios.FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
        }
    }
}
