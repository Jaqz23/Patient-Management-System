using SGP.Core.Domain.Common;

namespace SGP.Core.Domain.Entities
{
    public class Consultorio : BaseEntity
    {
        public string Nombre { get; set; }

        // Navigation Properties
        public List<Usuario> Usuarios { get; set; } = new();
        public List<Medico> Medicos { get; set; } = new();
        public List<Paciente> Pacientes { get; set; } = new();
        public List<Cita> Citas { get; set; } = new();
        public List<PruebaLaboratorio> PruebasLaboratorio { get; set; } = new();
    }
}
