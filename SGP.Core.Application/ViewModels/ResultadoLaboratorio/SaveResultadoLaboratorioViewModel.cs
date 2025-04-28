using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.ResultadoLaboratorio
{
    public class SaveResultadoLaboratorioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un resultado")]
        public string Resultado { get; set; }

        public bool Completado { get; set; } = false;

        public int PacienteId { get; set; }
   
        public int PruebaLaboratorioId { get; set; }

        public int CitaId { get; set; }

        public string Estado { get; set; }
    }
}
