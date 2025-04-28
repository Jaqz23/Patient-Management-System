using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.ResultadoLaboratorio;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.Services
{
    public class ResultadoLaboratorioService : IResultadoLaboratorioService
    {
        private readonly IResultadoLaboratorioRepository _resultadoRepository;
        private readonly ICitaRepository _citaRepository;

        public ResultadoLaboratorioService(IResultadoLaboratorioRepository resultadoRepository, ICitaRepository citaRepository)
        {
            _resultadoRepository = resultadoRepository;
            _citaRepository = citaRepository;
        }

        // Agregar un nuevo resultado de laboratorio
        public async Task<SaveResultadoLaboratorioViewModel> Add(SaveResultadoLaboratorioViewModel vm)
        {
            bool existe = await _resultadoRepository.ExisteResultadoParaCita(vm.CitaId, vm.PruebaLaboratorioId);
            if (existe)
            {
                throw new Exception("Ya existe un resultado para esta cita y prueba de laboratorio.");
            }

            ResultadoLaboratorio resultado = new()
            {
                Resultado = vm.Resultado,
                Completado = vm.Completado,
                PacienteId = vm.PacienteId,
                PruebaLaboratorioId = vm.PruebaLaboratorioId,
                CitaId = vm.CitaId
            };

            resultado = await _resultadoRepository.AddAsync(resultado);

            return new SaveResultadoLaboratorioViewModel
            {
                Id = resultado.Id,
                Resultado = resultado.Resultado,
                Completado = resultado.Completado,
                PacienteId = resultado.PacienteId,
                PruebaLaboratorioId = resultado.PruebaLaboratorioId,
                CitaId = resultado.CitaId
            };
        }

 
        public async Task Update(SaveResultadoLaboratorioViewModel vm)
        {
            var resultado = await _resultadoRepository.GetByIdAsync(vm.Id);
            if (resultado == null) return;

            resultado.Resultado = vm.Resultado;
            resultado.Completado = vm.Completado;

            await _resultadoRepository.UpdateAsync(resultado);
        }

       
        public async Task Delete(int id)
        {
            var resultado = await _resultadoRepository.GetByIdAsync(id);
            if (resultado != null)
            {
                await _resultadoRepository.DeleteAsync(resultado);
            }
        }

        
        public async Task<SaveResultadoLaboratorioViewModel> GetByIdSaveViewModel(int id)
        {
            var resultado = await _resultadoRepository.GetByIdAsync(id);
            if (resultado == null) return null;

            return new SaveResultadoLaboratorioViewModel
            {
                Id = resultado.Id,
                Resultado = resultado.Resultado,
                Completado = resultado.Completado,
                PacienteId = resultado.PacienteId,
                PruebaLaboratorioId = resultado.PruebaLaboratorioId,
                CitaId = resultado.CitaId
            };
        }

        
        public async Task<List<ResultadoLaboratorioViewModel>> GetAllViewModel()
        {
            var resultados = await _resultadoRepository.GetAllAsync();
            return resultados.Select(r => new ResultadoLaboratorioViewModel
            {
                Id = r.Id,
                Resultado = r.Resultado ?? "Pendiente",
                Completado = r.Completado,
                PacienteNombre = $"{r.Paciente?.Nombre} {r.Paciente?.Apellido}" ?? "Desconocido",
                PacienteCedula = r.Paciente?.Cedula ?? "N/A",
                PruebaNombre = r.PruebaLaboratorio?.Nombre ?? "No especificada",
            }).ToList();
        }

        
        public async Task<List<ResultadoLaboratorioViewModel>> GetResultadosPendientesByConsultorioAsync(int consultorioId)
        {
            var resultados = await _resultadoRepository.GetResultadosPendientesByConsultorioAsync(consultorioId);
           
            return resultados.Select(r => new ResultadoLaboratorioViewModel
            {
                Id = r.Id,
                Resultado = r.Resultado ?? "Pendiente",
                Completado = r.Completado,
                PacienteNombre = $"{r.Paciente?.Nombre} {r.Paciente?.Apellido}" ?? "Desconocido",
                PacienteCedula = r.Paciente?.Cedula ?? "N/A",
                PruebaNombre = r.PruebaLaboratorio?.Nombre ?? "No especificada",
                
            }).ToList();
        }

        
        public async Task<List<ResultadoLaboratorioViewModel>> GetResultadosByCedulaAsync(int consultorioId, string cedula)
        {
            var resultados = await _resultadoRepository.GetResultadosByCedulaAsync(consultorioId, cedula);
            return resultados.Select(r => new ResultadoLaboratorioViewModel
            {
                Id = r.Id,
                Resultado = r.Resultado ?? "Pendiente",
                Completado = r.Completado,
                PacienteNombre = $"{r.Paciente?.Nombre} {r.Paciente?.Apellido}" ?? "Desconocido",
                PacienteCedula = r.Paciente?.Cedula ?? "N/A",
                PruebaNombre = r.PruebaLaboratorio?.Nombre ?? "No especificada",
                
            }).ToList();
        }

        
        public async Task ReportarResultado(int id, string resultado)
        {
            var resultadoLab = await _resultadoRepository.GetByIdAsync(id);
            if (resultadoLab == null) return;

            resultadoLab.Resultado = resultado;
            resultadoLab.Completado = true;
            resultadoLab.Estado = EstadoResultado.Completado;

            await _resultadoRepository.UpdateAsync(resultadoLab);

            
            var resultadosCita = await _resultadoRepository.GetResultadosByCitaAsync(resultadoLab.CitaId);
            if (resultadosCita.All(r => r.Completado))
            {
                var cita = await _citaRepository.GetByIdAsync(resultadoLab.CitaId);
                if (cita != null)
                {
                    cita.Estado = EstadoCita.Completada;
                    await _citaRepository.UpdateAsync(cita);
                }
            }
        }
    }
}
