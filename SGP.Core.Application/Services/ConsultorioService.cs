using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Consultorio;
using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Services
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly IConsultorioRepository _consultorioRepository;

        public ConsultorioService(IConsultorioRepository consultorioRepository)
        {
            _consultorioRepository = consultorioRepository;
        }

        public async Task<SaveConsultorioViewModel> Add(SaveConsultorioViewModel vm)
        {
            var existeConsultorio = await _consultorioRepository.GetAllAsync();
            if (existeConsultorio.Any(c => c.Nombre.ToLower() == vm.Nombre.ToLower()))
            {
                throw new Exception("Ya existe un consultorio con este nombre.");
            }

            Consultorio consultorio = new()
            {
                Nombre = vm.Nombre
            };

            consultorio = await _consultorioRepository.AddAsync(consultorio);

            return new SaveConsultorioViewModel
            {
                Id = consultorio.Id,
                Nombre = consultorio.Nombre
            };
        }

        public async Task Update(SaveConsultorioViewModel vm)
        {
            var consultorio = await _consultorioRepository.GetByIdAsync(vm.Id);
            if (consultorio == null) return;

            var existeConsultorio = await _consultorioRepository.GetAllAsync();
            if (existeConsultorio.Any(c => c.Nombre.ToLower() == vm.Nombre.ToLower() && c.Id != vm.Id))
            {
                throw new Exception("Ya existe otro consultorio con este nombre.");
            }

            consultorio.Nombre = vm.Nombre;

            await _consultorioRepository.UpdateAsync(consultorio);
        }

        public async Task Delete(int id)
        {
            var consultorio = await _consultorioRepository.GetByIdAsync(id);
            if (consultorio == null) return;

            if (consultorio.Usuarios.Any() || consultorio.Medicos.Any() || consultorio.Pacientes.Any())
            {
                throw new Exception("No se puede eliminar un consultorio que tenga médicos, pacientes o usuarios asignados.");
            }

            await _consultorioRepository.DeleteAsync(consultorio);
        }

        public async Task<SaveConsultorioViewModel> GetByIdSaveViewModel(int id)
        {
            var consultorio = await _consultorioRepository.GetByIdAsync(id);
            if (consultorio == null) return null;

            return new SaveConsultorioViewModel
            {
                Id = consultorio.Id,
                Nombre = consultorio.Nombre
            };
        }

        public async Task<ConsultorioViewModel> GetByNombre(string nombre)
        {
            var consultorio = await _consultorioRepository.GetByNombre(nombre);
            if (consultorio == null) return null;

            return new ConsultorioViewModel
            {
                Id = consultorio.Id,
                Nombre = consultorio.Nombre
            };
        }

        public async Task<List<ConsultorioViewModel>> GetAllViewModel()
        {
            var consultorios = await _consultorioRepository.GetAllAsync();
            return consultorios.Select(c => new ConsultorioViewModel
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();
        }


    }
}
