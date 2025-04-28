using Microsoft.EntityFrameworkCore;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;
using SGP.Infrastucture.Persistence.Contexts;


namespace SGP.Infrastucture.Persistence.Repositories
{
    public class CitaRepository : GenericRepository<Cita>, ICitaRepository
    {
        private readonly ApplicationContext _dbContext;

        public CitaRepository(ApplicationContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        
        public async Task<List<Cita>> GetCitasByConsultorioAsync(int consultorioId)
        {
            return await _dbContext.Citas
                .Where(c => c.ConsultorioId == consultorioId)
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();
        }

        
        public async Task<List<Cita>> GetCitasByEstadoAsync(int consultorioId, EstadoCita estado)
        {
            return await _dbContext.Citas
                .Where(c => c.ConsultorioId == consultorioId && c.Estado == estado)
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();
        }

        
        public async Task AsignarPruebasALaCitaAsync(int citaId, List<int> pruebaIds)
        {
            var cita = await _dbContext.Citas.FindAsync(citaId);
            if (cita == null) return;

            foreach (var pruebaId in pruebaIds)
            {
                var resultado = new ResultadoLaboratorio
                {
                    CitaId = citaId,
                    PacienteId = cita.PacienteId,
                    PruebaLaboratorioId = pruebaId,
                    Estado = EstadoResultado.Pendiente,
                    ConsultorioId = cita.ConsultorioId
                };

                await _dbContext.ResultadosLaboratorio.AddAsync(resultado);
            }

            cita.Estado = EstadoCita.PendienteDeResultados;
            await _dbContext.SaveChangesAsync();
        }

        
        public async Task<List<ResultadoLaboratorio>> GetResultadosByCitaAsync(int citaId)
        {
            return await _dbContext.ResultadosLaboratorio
                .Where(r => r.CitaId == citaId)
                .Include(r => r.PruebaLaboratorio)
                .ToListAsync();
        }
    }
}
