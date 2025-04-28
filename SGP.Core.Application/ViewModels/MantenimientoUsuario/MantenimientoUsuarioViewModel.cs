using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.ViewModels.MantenimientoUsuario
{
    public class MantenimientoUsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public RolUsuario Rol { get; set; }
    }
}
