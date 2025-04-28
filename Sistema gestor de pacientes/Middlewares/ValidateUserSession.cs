using SGP.Core.Application.Helpers;
using SGP.Core.Application.ViewModels.Usuario;

namespace Sistema_gestor_de_pacientes.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            UsuarioViewModel usuarioViewModel = _httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");

            if (usuarioViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}
