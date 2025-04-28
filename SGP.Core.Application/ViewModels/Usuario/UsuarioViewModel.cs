
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.ViewModels.Usuario
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public int ConsultorioId { get; set; }
        public RolUsuario Rol { get; set; }
    }
}
