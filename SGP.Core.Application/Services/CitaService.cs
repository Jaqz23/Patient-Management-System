using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Cita;
using SGP.Core.Application.ViewModels.ResultadoLaboratorio;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;
using SGP.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;


namespace SGP.Core.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IResultadoLaboratorioRepository _resultadoLaboratorioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel? _usuarioActual;

        public CitaService(ICitaRepository citaRepository,
                           IResultadoLaboratorioRepository resultadoLaboratorioRepository,
                           IHttpContextAccessor httpContextAccessor)
        {
            _citaRepository = citaRepository;
            _resultadoLaboratorioRepository = resultadoLaboratorioRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = httpContextAccessor.HttpContext?.Session.Get<UsuarioViewModel>("usuario");
        }

        
        public async Task<List<CitaViewModel>> GetAllViewModel()
        {
            if (_usuarioActual == null) return new List<CitaViewModel>();

            var citas = await _citaRepository.GetCitasByConsultorioAsync(_usuarioActual.ConsultorioId);

            return citas.Select(c => new CitaViewModel
            {
                Id = c.Id,
                Fecha = c.Fecha,
                Hora = c.Hora,
                Causa = c.Causa,
                Estado = c.Estado.ToString(),
                PacienteNombre = $"{c.Paciente?.Nombre ?? "Desconocido"} {c.Paciente?.Apellido ?? ""}",
                MedicoNombre = $"{c.Medico?.Nombre ?? "Desconocido"} {c.Medico?.Apellido ?? ""}"
            }).ToList();
        }

        public async Task<List<CitaViewModel>> GetCitasByConsultorioAsync(int consultorioId)
        {
            var citas = await _citaRepository.GetCitasByConsultorioAsync(consultorioId);

            return citas.Select(c => new CitaViewModel
            {
                Id = c.Id,
                Fecha = c.Fecha,
                Hora = c.Hora,
                Causa = c.Causa,
                Estado = c.Estado.ToString(),
                PacienteNombre = $"{c.Paciente?.Nombre ?? "Desconocido"} {c.Paciente?.Apellido ?? ""}",
                MedicoNombre = $"{c.Medico?.Nombre ?? "Desconocido"} {c.Medico?.Apellido ?? ""}"
            }).ToList();
        }

       
        public async Task<List<CitaViewModel>> GetCitasByEstadoAsync(int consultorioId, EstadoCita estado)
        {
            var citas = await _citaRepository.GetCitasByEstadoAsync(consultorioId, estado);

            return citas.Select(c => new CitaViewModel
            {
                Id = c.Id,
                Fecha = c.Fecha,
                Hora = c.Hora,
                Causa = c.Causa,
                Estado = c.Estado.ToString(),
                PacienteNombre = $"{c.Paciente?.Nombre ?? "Desconocido"} {c.Paciente?.Apellido ?? ""}",
                MedicoNombre = $"{c.Medico?.Nombre ?? "Desconocido"} {c.Medico?.Apellido ?? ""}"
            }).ToList();
        }

        
        public async Task AsignarPruebasALaCita(int citaId, List<int> pruebaIds)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null) return;

            await _citaRepository.AsignarPruebasALaCitaAsync(citaId, pruebaIds);

            cita.Estado = EstadoCita.PendienteDeResultados;
            await _citaRepository.UpdateAsync(cita);
        }

        
        public async Task CambiarEstadoCita(int citaId, EstadoCita nuevoEstado)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null) return;

            cita.Estado = nuevoEstado;
            await _citaRepository.UpdateAsync(cita);
        }

        
        public async Task<List<ResultadoLaboratorioViewModel>> GetResultadosPorCita(int citaId)
        {
            var resultados = await _resultadoLaboratorioRepository.GetResultadosByCitaAsync(citaId);

            return resultados.Select(r => new ResultadoLaboratorioViewModel
            {
                Id = r.Id,
                PruebaNombre = r.PruebaLaboratorio.Nombre,
                Estado = r.Completado ? "Completada" : "Pendiente",
                Resultado = r.Resultado
            }).ToList();
        }

        public async Task<List<ResultadoLaboratorioViewModel>> GetResultadosCompletadosPorCita(int citaId)
        {
            var resultados = await _resultadoLaboratorioRepository.GetResultadosCompletadosByCitaAsync(citaId);

            return resultados.Select(r => new ResultadoLaboratorioViewModel
            {
                Id = r.Id,
                PruebaNombre = r.PruebaLaboratorio.Nombre,
                Resultado = string.IsNullOrEmpty(r.Resultado) ? "No disponible" : r.Resultado
            }).ToList();
        }

        public async Task<SaveCitaViewModel?> GetByIdSaveViewModel(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null) return null;

            return new SaveCitaViewModel
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                Causa = cita.Causa,
                Estado = cita.Estado,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                ConsultorioId = cita.ConsultorioId
            };
        }

        
        public async Task<SaveCitaViewModel> Add(SaveCitaViewModel vm)
        {
            if (_usuarioActual == null) return vm;

            Cita cita = new()
            {
                Fecha = vm.Fecha,
                Hora = vm.Hora,
                Causa = vm.Causa,
                Estado = EstadoCita.PendienteDeConsulta,
                PacienteId = vm.PacienteId,
                MedicoId = vm.MedicoId,
                ConsultorioId = _usuarioActual.ConsultorioId
            };

            cita = await _citaRepository.AddAsync(cita);

            return new SaveCitaViewModel
            {
                Id = cita.Id,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                Causa = cita.Causa,
                Estado = cita.Estado,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                ConsultorioId = cita.ConsultorioId
            };
        }

        
        public async Task Update(SaveCitaViewModel vm)
        {
            var cita = await _citaRepository.GetByIdAsync(vm.Id);
            if (cita == null) return;

            cita.Fecha = vm.Fecha;
            cita.Hora = vm.Hora;
            cita.Causa = vm.Causa;
            cita.Estado = vm.Estado;
            cita.PacienteId = vm.PacienteId;
            cita.MedicoId = vm.MedicoId;

            await _citaRepository.UpdateAsync(cita);
        }

       
        public async Task Delete(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null) return;

            // Verificar si la cita tiene resultados de laboratorio asociados
            var resultados = await _resultadoLaboratorioRepository.GetResultadosByCitaAsync(id);

            if (resultados.Any())
            {
                // Eliminar todos los resultados de laboratorio relacionados con la cita
                foreach (var resultado in resultados)
                {
                    await _resultadoLaboratorioRepository.DeleteAsync(resultado);
                }
            }

            await _citaRepository.DeleteAsync(cita);
        }
    }
}
