using SGP.Core.Application.Helpers;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Enums;

namespace Sistema_gestor_de_pacientes.Middlewares
{
    public class ValidateUserRole
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioViewModel? _usuarioActual;

        public ValidateUserRole(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuarioActual = _httpContextAccessor.HttpContext?.Session.Get<UsuarioViewModel>("usuario");
        }

        public bool IsAdmin()
        {
            return _usuarioActual?.Rol == RolUsuario.Administrador;
        }

        public bool IsAsistente()
        {
            return _usuarioActual?.Rol == RolUsuario.Asistente;
        }
    }
}
