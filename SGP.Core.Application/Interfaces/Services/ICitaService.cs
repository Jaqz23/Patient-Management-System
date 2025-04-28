using SGP.Core.Application.ViewModels.Cita;
using SGP.Core.Application.ViewModels.ResultadoLaboratorio;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface ICitaService : IGenericService<SaveCitaViewModel, CitaViewModel>
    {
        Task<List<CitaViewModel>> GetCitasByConsultorioAsync(int consultorioId);
        Task<List<CitaViewModel>> GetCitasByEstadoAsync(int consultorioId, EstadoCita estado);
        Task AsignarPruebasALaCita(int citaId, List<int> pruebaIds);
        Task CambiarEstadoCita(int citaId, EstadoCita nuevoEstado);
        Task<List<ResultadoLaboratorioViewModel>> GetResultadosPorCita(int citaId);
        Task<List<ResultadoLaboratorioViewModel>> GetResultadosCompletadosPorCita(int citaId);

    }
}
