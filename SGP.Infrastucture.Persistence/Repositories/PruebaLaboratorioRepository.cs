using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class PruebaLaboratorioRepository : GenericRepository<PruebaLaboratorio>, IPruebaLaboratorioRepository
    {
        private readonly ApplicationContext _dbContext;

        public PruebaLaboratorioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       
    }
}
