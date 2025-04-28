
namespace SGP.Core.Application.ViewModels.Paciente
{
    public class PacienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool EsFumador { get; set; }
        public string Alergias { get; set; }
        public string Foto { get; set; }
        public int ConsultorioId { get; set; }
    }
}
