using SGP.Core.Application.ViewModels.ResultadoLaboratorio;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IResultadoLaboratorioService : IGenericService<SaveResultadoLaboratorioViewModel, ResultadoLaboratorioViewModel>
    {
        Task<List<ResultadoLaboratorioViewModel>> GetResultadosPendientesByConsultorioAsync(int consultorioId);
        Task<List<ResultadoLaboratorioViewModel>> GetResultadosByCedulaAsync(int consultorioId, string cedula);
        Task ReportarResultado(int id, string resultado);
    }
}
