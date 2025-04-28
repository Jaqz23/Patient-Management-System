using System.ComponentModel.DataAnnotations;
using SGP.Core.Domain.Enums;

namespace SGP.Core.Application.ViewModels.Cita
{
    public class SaveCitaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe ingresar una hora")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "Debe ingresar la causa de la cita")]
        [DataType(DataType.Text)]
        public string? Causa { get; set; }

        public EstadoCita Estado { get; set; } = EstadoCita.PendienteDeConsulta;

        [Required(ErrorMessage = "Debe seleccionar un paciente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un paciente válido")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un médico")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un médico válido")]
        public int MedicoId { get; set; }

        public int ConsultorioId { get; set; }
    }
}
