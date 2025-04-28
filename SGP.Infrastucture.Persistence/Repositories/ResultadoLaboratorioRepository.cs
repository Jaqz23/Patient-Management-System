using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;
using SGP.Infrastucture.Persistence.Contexts;


namespace SGP.Infrastucture.Persistence.Repositories
{
    public class ResultadoLaboratorioRepository : GenericRepository<ResultadoLaboratorio>, IResultadoLaboratorioRepository
    {
        private readonly ApplicationContext _dbContext;

        public ResultadoLaboratorioRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<List<ResultadoLaboratorio>> GetResultadosByCitaAsync(int citaId)
        {
            return await _dbContext.ResultadosLaboratorio
                .Where(r => r.CitaId == citaId)
                .Include(r => r.PruebaLaboratorio)
                .ToListAsync();
        }

       
        public async Task<List<ResultadoLaboratorio>> GetResultadosPendientesByConsultorioAsync(int consultorioId)
        {
            return await _dbContext.ResultadosLaboratorio
                .Include(r => r.Paciente)
                .Include(r => r.PruebaLaboratorio)
                .Where(r => r.ConsultorioId == consultorioId && r.Estado == EstadoResultado.Pendiente)
                .ToListAsync();
        }

        
        public async Task<List<ResultadoLaboratorio>> GetResultadosByCedulaAsync(int consultorioId, string cedula)
        {
            return await _dbContext.ResultadosLaboratorio
                .Include(r => r.Paciente)
                .Include(r => r.PruebaLaboratorio)
                .Where(r => r.ConsultorioId == consultorioId &&
                            r.Paciente != null &&
                            r.Paciente.Cedula == cedula &&
                            r.Estado == EstadoResultado.Pendiente)
                .ToListAsync();
        }

        
        public async Task<List<ResultadoLaboratorio>> GetResultadosCompletadosByCitaAsync(int citaId)
        {
            return await _dbContext.ResultadosLaboratorio
                .Include(r => r.PruebaLaboratorio)
                .Where(r => r.CitaId == citaId && r.Completado == true)
                .ToListAsync();
        }

        
        public async Task<bool> ExisteResultadoParaCita(int citaId, int pruebaLaboratorioId)
        {
            return await _dbContext.ResultadosLaboratorio
                .AnyAsync(r => r.CitaId == citaId && r.PruebaLaboratorioId == pruebaLaboratorioId);
        }
    }
}
