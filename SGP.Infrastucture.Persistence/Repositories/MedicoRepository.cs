using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class MedicoRepository : GenericRepository<Medico>, IMedicoRepository
    {
        private readonly ApplicationContext _dbContext;

        public MedicoRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsByCorreoAsync(string correo)
        {
            return await _dbContext.Medicos.AnyAsync(m => m.Correo == correo);
        }

        public async Task<bool> ExistsByTelefono(string telefono)
        {
            return await _dbContext.Medicos.AnyAsync(m => m.Telefono == telefono);
        }

        public async Task<bool> ExistsByCedulaAsync(string cedula)
        {
            return await _dbContext.Medicos.AnyAsync(m => m.Cedula == cedula);
        }

        public async Task<List<Medico>> GetMedicosByConsultorioAsync(int consultorioId)
        {
            return await _dbContext.Medicos
                .Where(m => m.ConsultorioId == consultorioId)
                .ToListAsync();
        }
    }
}
