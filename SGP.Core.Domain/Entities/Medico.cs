using SGP.Core.Domain.Common;

namespace SGP.Core.Domain.Entities
{
    public class Medico : BaseEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string Foto { get; set; }

        // Foreign Key
        public int ConsultorioId { get; set; }

        // Navigation Properties
        public Consultorio? Consultorio { get; set; }
        public List<Cita> Citas { get; set; } = new();
    }
}
