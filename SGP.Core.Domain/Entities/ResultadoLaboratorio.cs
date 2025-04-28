using SGP.Core.Domain.Common;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Domain.Entities
{
    public class ResultadoLaboratorio : BaseEntity
    {
        public string? Resultado { get; set; } // Puede ser nulo hasta completarse
        public bool Completado { get; set; } = false;
        public EstadoResultado Estado { get; set; } = EstadoResultado.Pendiente; 

        // Foreign Keys
        public int PacienteId { get; set; }
        public int PruebaLaboratorioId { get; set; }
        public int CitaId { get; set; }
        public int ConsultorioId { get; set; }

        // Navigation Properties
        public Paciente? Paciente { get; set; }
        public PruebaLaboratorio? PruebaLaboratorio { get; set; }
        public Cita? Cita { get; set; }
    }
}
