using Microsoft.AspNetCore.Http;
using SGP.Core.Application.Helpers;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.PruebaLaboratorio;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;

namespace SGP.Core.Application.Services
{
    public class PruebaLaboratorioService : IPruebaLaboratorioService
    {
        private readonly IPruebaLaboratorioRepository _pruebaLaboratorioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioActual;

        public PruebaLaboratorioService(IPruebaLaboratorioRepository pruebaLaboratorioRepository, IHttpContextAccessor httpContextAccessor)
        {
            _pruebaLaboratorioRepository = pruebaLaboratorioRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
        }

        public async Task<SavePruebaLaboratorioViewModel> Add(SavePruebaLaboratorioViewModel vm)
        {
            PruebaLaboratorio prueba = new()
            {
                Nombre = vm.Nombre,
                ConsultorioId = _usuarioActual.ConsultorioId // Se asigna el consultorio del admin logueado
            };

            prueba = await _pruebaLaboratorioRepository.AddAsync(prueba);

            return new SavePruebaLaboratorioViewModel
            {
                Id = prueba.Id,
                Nombre = prueba.Nombre,
                ConsultorioId = prueba.ConsultorioId
            };
        }

        public async Task Update(SavePruebaLaboratorioViewModel vm)
        {
            var prueba = await _pruebaLaboratorioRepository.GetByIdAsync(vm.Id);
            if (prueba == null || prueba.ConsultorioId != _usuarioActual.ConsultorioId) return;

            prueba.Nombre = vm.Nombre;

            await _pruebaLaboratorioRepository.UpdateAsync(prueba);
        }

        public async Task Delete(int id)
        {
            var prueba = await _pruebaLaboratorioRepository.GetByIdAsync(id);
            if (prueba != null && prueba.ConsultorioId == _usuarioActual.ConsultorioId)
            {
                await _pruebaLaboratorioRepository.DeleteAsync(prueba);
            }
        }

        public async Task<SavePruebaLaboratorioViewModel> GetByIdSaveViewModel(int id)
        {
            var prueba = await _pruebaLaboratorioRepository.GetByIdAsync(id);
            if (prueba == null || prueba.ConsultorioId != _usuarioActual.ConsultorioId) return null;

            return new SavePruebaLaboratorioViewModel
            {
                Id = prueba.Id,
                Nombre = prueba.Nombre,
                ConsultorioId = prueba.ConsultorioId
            };
        }

        public async Task<List<PruebaLaboratorioViewModel>> GetAllViewModel()
        {
            var pruebas = await _pruebaLaboratorioRepository.GetAllAsync();
            return pruebas.Where(p => p.ConsultorioId == _usuarioActual.ConsultorioId).Select(p => new PruebaLaboratorioViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList();
        }
    }
}
