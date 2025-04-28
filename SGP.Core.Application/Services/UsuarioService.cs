using SGP.Core.Application.Interfaces.Repositories;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Entities;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> ExistsByNombreUsuario(string nombreUsuario)
        {
            return await _usuarioRepository.ExistsByNombreUsuario(nombreUsuario);
        }


        public async Task<bool> ExistsByCorreo(string correo)
        {
            return await _usuarioRepository.ExistsByCorreo(correo);
        }


        public async Task<UsuarioViewModel> Login(LoginViewModel loginVm)
        {
            UsuarioViewModel usuarioVm = new();
            Usuario usuario = await _usuarioRepository.LoginAsync(loginVm);

            if (usuario == null)
            {
                return null;
            }

            usuarioVm.Id = usuario.Id;
            usuarioVm.Nombre = usuario.Nombre;
            usuarioVm.Apellido = usuario.Apellido;
            usuarioVm.Correo = usuario.Correo;
            usuarioVm.NombreUsuario = usuario.NombreUsuario;
            usuarioVm.Contraseña = usuario.Contraseña;
            usuarioVm.ConsultorioId = usuario.ConsultorioId;
            usuarioVm.Rol = usuario.Rol;

            return usuarioVm;
        }


        public async Task<SaveUsuarioViewModel> Add(SaveUsuarioViewModel vm)
        {
            // Validar si el nombre y correo de usuario ya existe
            bool usuarioExistente = await _usuarioRepository.ExistsByNombreUsuario(vm.NombreUsuario);
            bool correoExistente = await _usuarioRepository.ExistsByCorreo(vm.Correo);
            

            Usuario usuario = new()
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                Correo = vm.Correo,
                NombreUsuario = vm.NombreUsuario,
                Contraseña = vm.Contraseña,
                Rol = RolUsuario.Administrador,
                ConsultorioId = vm.ConsultorioId
            };

            usuario = await _usuarioRepository.AddAsync(usuario);

            return new SaveUsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Contraseña = usuario.Contraseña,
                ConsultorioId = usuario.ConsultorioId
            };
        }

        public async Task Update(SaveUsuarioViewModel vm)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(vm.Id);
            if (usuario == null) return;

            usuario.Nombre = vm.Nombre;
            usuario.Apellido = vm.Apellido;
            usuario.Correo = vm.Correo;
            usuario.NombreUsuario = vm.NombreUsuario;
            usuario.Contraseña = vm.Contraseña;

            await _usuarioRepository.UpdateAsync(usuario);
        }


        public async Task Delete(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario != null)
            {
                await _usuarioRepository.DeleteAsync(usuario);
            }
        }


        public async Task<SaveUsuarioViewModel> GetByIdSaveViewModel(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new SaveUsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Contraseña = usuario.Contraseña,
                ConsultorioId = usuario.ConsultorioId
            };
        }

        public async Task<List<UsuarioViewModel>> GetAllViewModel()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                NombreUsuario = u.NombreUsuario,
                Contraseña = u.Contraseña,
                ConsultorioId = u.ConsultorioId
            }).ToList();
        }
    }
}
