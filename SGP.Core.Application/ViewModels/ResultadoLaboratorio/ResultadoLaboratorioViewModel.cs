
namespace SGP.Core.Application.ViewModels.ResultadoLaboratorio
{
    public class ResultadoLaboratorioViewModel
    {
        public int Id { get; set; }
        public string Resultado { get; set; }
        public bool Completado { get; set; }
        public string PacienteNombre { get; set; }
        public string PacienteCedula { get; set; }
        public string PruebaNombre { get; set; }
        public string Estado { get; set; }
    }
}
