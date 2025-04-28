
namespace SGP.Core.Application.ViewModels.Cita
{
    public class CitaViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Causa { get; set; }
        public string Estado { get; set; }
        public string PacienteNombre { get; set; }
        public string MedicoNombre { get; set; }
    }
}
