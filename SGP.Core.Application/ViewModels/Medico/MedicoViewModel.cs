
namespace SGP.Core.Application.ViewModels.Medico
{
    public class MedicoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Foto { get; set; }
        public int ConsultorioId { get; set; }
    }
}
