using SGP.Core.Domain.Common;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public RolUsuario Rol { get; set; } 

        // Foreign Key
        public int ConsultorioId { get; set; }

        // Navigation Property
        public Consultorio? Consultorio { get; set; }
    }
}
