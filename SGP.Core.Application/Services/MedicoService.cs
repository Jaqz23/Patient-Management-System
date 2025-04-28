using Microsoft.AspNetCore.Http;
using SGP.Core.Application.Helpers;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Medico;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;


namespace SGP.Core.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioActual;

        public MedicoService(IMedicoRepository medicoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _medicoRepository = medicoRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
        }

        
        public async Task<bool> ExistsByCedula(string cedula)
        {
            return await _medicoRepository.ExistsByCedulaAsync(cedula);
        }

        public async Task<bool> ExistsByCorreo(string correo)
        {
            return await _medicoRepository.ExistsByCorreoAsync(correo);
        }

        public async Task<bool> ExistsByTelefono(string telefono)
        {
            return await _medicoRepository.ExistsByTelefono(telefono);
        }

        public async Task<SaveMedicoViewModel> Add(SaveMedicoViewModel vm)
        {
            Medico medico = new()
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                Correo = vm.Correo,
                Cedula = vm.Cedula,
                Telefono = vm.Telefono,
                Foto = vm.Foto,
                ConsultorioId = _usuarioActual.ConsultorioId
            };

            medico = await _medicoRepository.AddAsync(medico);

            return new SaveMedicoViewModel
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Correo = medico.Correo,
                Cedula = medico.Cedula,
                Telefono = medico.Telefono,
                Foto = medico.Foto,
                ConsultorioId = medico.ConsultorioId
            };
        }

        public async Task Update(SaveMedicoViewModel vm)
        {
            Medico medico = await _medicoRepository.GetByIdAsync(vm.Id);
            if (medico == null) return;

            medico.Nombre = vm.Nombre;
            medico.Apellido = vm.Apellido;
            medico.Correo = vm.Correo;
            medico.Cedula = vm.Cedula;
            medico.Telefono = vm.Telefono;

            if (!string.IsNullOrEmpty(vm.Foto))
            {
                medico.Foto = vm.Foto;
            }

            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task Delete(int id)
        {
            Medico medico = await _medicoRepository.GetByIdAsync(id);
            if (medico != null)
            {
                await _medicoRepository.DeleteAsync(medico);
            }
        }

        public async Task<SaveMedicoViewModel> GetByIdSaveViewModel(int id)
        {
            Medico medico = await _medicoRepository.GetByIdAsync(id);
            if (medico == null) return null;

            return new SaveMedicoViewModel
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Correo = medico.Correo,
                Cedula = medico.Cedula,
                Telefono = medico.Telefono,
                Foto = medico.Foto,
                ConsultorioId = medico.ConsultorioId
            };
        }

        public async Task<List<MedicoViewModel>> GetAllViewModel()
        {
            var medicos = await _medicoRepository.GetAllAsync();

            return medicos.Where(m => m.ConsultorioId == _usuarioActual.ConsultorioId).Select(m => new MedicoViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Correo = m.Correo,
                    Cedula = m.Cedula,
                    Telefono = m.Telefono,
                    Foto = m.Foto
                })
                .ToList();
        }

        public async Task<List<MedicoViewModel>> GetMedicosByConsultorioAsync(int consultorioId)
        {
            var medicos = await _medicoRepository.GetMedicosByConsultorioAsync(consultorioId);
            return medicos.Select(m => new MedicoViewModel
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                Correo = m.Correo,
                Cedula = m.Cedula,
                Telefono = m.Telefono,
                Foto = m.Foto
            }).ToList();
        }
    }

}
