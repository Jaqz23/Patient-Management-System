using SGP.Core.Domain.Common;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Domain.Entities
{
    public class Cita : BaseEntity
    {
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Causa { get; set; }
        public EstadoCita Estado { get; set; } = EstadoCita.PendienteDeConsulta;

        // Foreign Keys
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int ConsultorioId { get; set; }

        // Navigation Properties
        public Paciente? Paciente { get; set; }
        public Medico? Medico { get; set; }
        public Consultorio? Consultorio { get; set; }
        public List<ResultadoLaboratorio> Resultados { get; set; } = new();
    }
}
