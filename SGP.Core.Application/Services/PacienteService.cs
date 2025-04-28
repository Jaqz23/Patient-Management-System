using Microsoft.AspNetCore.Http;
using SGP.Core.Application.Helpers;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Paciente;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioActual;

        public PacienteService(IPacienteRepository pacienteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _pacienteRepository = pacienteRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
        }

        public async Task<bool> ExistsByCedulaAsync(string cedula)
        {
            return await _pacienteRepository.ExistsByCedulaAsync(cedula);
        }

        public async Task<bool> ExistsByTelefonoAsync(string telefono)
        {
            return await _pacienteRepository.ExistsByTelefonoAsync(telefono);
        }

        public async Task<SavePacienteViewModel> Add(SavePacienteViewModel vm)
        {
            Paciente paciente = new()
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                Telefono = vm.Telefono,
                Direccion = vm.Direccion,
                Cedula = vm.Cedula,
                FechaNacimiento = vm.FechaNacimiento,
                EsFumador = vm.EsFumador,
                Alergias = vm.Alergias,
                Foto = vm.Foto,
                ConsultorioId = _usuarioActual.ConsultorioId
            };

            paciente = await _pacienteRepository.AddAsync(paciente);

            return new SavePacienteViewModel
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                Cedula = paciente.Cedula,
                FechaNacimiento = paciente.FechaNacimiento,
                EsFumador = paciente.EsFumador,
                Alergias = paciente.Alergias,
                Foto = paciente.Foto,
                ConsultorioId = paciente.ConsultorioId
            };
        }

        public async Task Update(SavePacienteViewModel vm)
        {
            Paciente paciente = await _pacienteRepository.GetByIdAsync(vm.Id);
            if (paciente == null) return;

            paciente.Nombre = vm.Nombre;
            paciente.Apellido = vm.Apellido;
            paciente.Telefono = vm.Telefono;
            paciente.Direccion = vm.Direccion;
            paciente.Cedula = vm.Cedula;
            paciente.FechaNacimiento = vm.FechaNacimiento;
            paciente.EsFumador = vm.EsFumador;
            paciente.Alergias = vm.Alergias;

            if (!string.IsNullOrEmpty(vm.Foto))
            {
                paciente.Foto = vm.Foto;
            }

            await _pacienteRepository.UpdateAsync(paciente);
        }

        public async Task Delete(int id)
        {
            Paciente paciente = await _pacienteRepository.GetByIdAsync(id);
            if (paciente != null)
            {
                await _pacienteRepository.DeleteAsync(paciente);
            }
        }

        public async Task<SavePacienteViewModel> GetByIdSaveViewModel(int id)
        {
            Paciente paciente = await _pacienteRepository.GetByIdAsync(id);
            if (paciente == null) return null;

            return new SavePacienteViewModel
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                Cedula = paciente.Cedula,
                FechaNacimiento = paciente.FechaNacimiento,
                EsFumador = paciente.EsFumador,
                Alergias = paciente.Alergias,
                Foto = paciente.Foto,
                ConsultorioId = paciente.ConsultorioId
            };
        }

        public async Task<List<PacienteViewModel>> GetAllViewModel()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();

            return pacientes
                .Where(p => p.ConsultorioId == _usuarioActual.ConsultorioId)
                .Select(p => new PacienteViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Telefono = p.Telefono,
                    Direccion = p.Direccion,
                    Cedula = p.Cedula,
                    FechaNacimiento = p.FechaNacimiento,
                    EsFumador = p.EsFumador,
                    Alergias = p.Alergias,
                    Foto = p.Foto
                }).ToList();
        }

        public async Task<List<PacienteViewModel>> GetPacientesByConsultorioAsync(int consultorioId)
        {
            var pacientes = await _pacienteRepository.GetPacientesByConsultorioAsync(consultorioId);
            return pacientes.Select(p => new PacienteViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Telefono = p.Telefono,
                Direccion = p.Direccion,
                Cedula = p.Cedula,
                FechaNacimiento = p.FechaNacimiento,
                EsFumador = p.EsFumador,
                Alergias = p.Alergias,
                Foto = p.Foto
            }).ToList();
        }
    }

}
