using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Infrastucture.Persistence.Contexts;

namespace SGP.Infrastucture.Persistence.Repositories
{
    public class PacienteRepository : GenericRepository<Paciente>, IPacienteRepository
    {
        private readonly ApplicationContext _dbContext;

        public PacienteRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsByCedulaAsync(string cedula)
        {
            return await _dbContext.Pacientes.AnyAsync(p => p.Cedula == cedula);
        }

        public async Task<bool> ExistsByTelefonoAsync(string telefono)
        {
            return await _dbContext.Pacientes.AnyAsync(p => p.Telefono == telefono);
        }

        public async Task<List<Paciente>> GetPacientesByConsultorioAsync(int consultorioId)
        {
            return await _dbContext.Pacientes
                .Where(p => p.ConsultorioId == consultorioId)
                .ToListAsync();
        }
    }
}
