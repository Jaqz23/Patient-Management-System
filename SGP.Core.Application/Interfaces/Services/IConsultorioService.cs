using SGP.Core.Application.ViewModels.Consultorio;

namespace SGP.Core.Application.Interfaces.Services
{
    public interface IConsultorioService : IGenericService<SaveConsultorioViewModel, ConsultorioViewModel>
    {
        Task<ConsultorioViewModel> GetByNombre(string nombre);
    }
}
