using Microsoft.AspNetCore.Http;
using SGP.Core.Application.Helpers;
using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.MantenimientoUsuario;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;


namespace SGP.Core.Application.Services
{
    public class MantenimientoUsuarioService : IMantenimientoUsuarioService
    {
        private readonly IMantenimientoUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel _usuarioActual;

        public MantenimientoUsuarioService(IMantenimientoUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
        }


        public async Task<bool> ExistsByNombreUsuario(string nombreUsuario)
        {
            return await _usuarioRepository.ExistsByNombreUsuarioAsync(nombreUsuario);
        }


        public async Task<bool> ExistsByCorreo(string correo)
        {
            return await _usuarioRepository.ExistsByCorreoAsync(correo);
        }


        public async Task<SaveMantenimientoUsuarioViewModel> Add(SaveMantenimientoUsuarioViewModel vm)
        {

            if (_usuarioActual == null)
            {
                throw new Exception("No se ha encontrado el usuario administrador.");
            }

            Usuario usuario = new()
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                Correo = vm.Correo,
                NombreUsuario = vm.NombreUsuario,
                Contraseña = PasswordEncryptation.ComputeSha256Hash(vm.Contraseña),
                Rol = vm.Rol,
                ConsultorioId = _usuarioActual.ConsultorioId // Se asigna al consultorio del admin logueado
            };

            usuario = await _usuarioRepository.AddAsync(usuario);

            return new SaveMantenimientoUsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol
            };

        }


        public async Task Update(SaveMantenimientoUsuarioViewModel vm)
        {
            Usuario usuario = await _usuarioRepository.GetByIdAsync(vm.Id);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            usuario.Nombre = vm.Nombre;
            usuario.Apellido = vm.Apellido;
            usuario.Correo = vm.Correo;
            usuario.NombreUsuario = vm.NombreUsuario;
            usuario.Contraseña = PasswordEncryptation.ComputeSha256Hash(vm.Contraseña);
            usuario.Rol = vm.Rol;

            await _usuarioRepository.UpdateAsync(usuario);
        }


        public async Task Delete(int id)
        {
            Usuario usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return;

            await _usuarioRepository.DeleteAsync(usuario);
        }


        public async Task<SaveMantenimientoUsuarioViewModel> GetByIdSaveViewModel(int id)
        {
            Usuario usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new SaveMantenimientoUsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol
            };
        }


        public async Task<List<MantenimientoUsuarioViewModel>> GetAllViewModel()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Where(u => u.ConsultorioId == _usuarioActual.ConsultorioId).Select(u => new MantenimientoUsuarioViewModel
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                NombreUsuario = u.NombreUsuario,
                Rol = u.Rol
            }).ToList();
        }


        public async Task<List<MantenimientoUsuarioViewModel>> GetUsuariosByConsultorioAsync(int consultorioId)
        {
            var usuarios = await _usuarioRepository.GetUsuariosByConsultorioAsync(consultorioId);
            return usuarios.Select(u => new MantenimientoUsuarioViewModel
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                NombreUsuario = u.NombreUsuario,
                Rol = u.Rol
            }).ToList();
        }
    }
}
