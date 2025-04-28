using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.Consultorio
{
    public class SaveConsultorioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del consultorio")]
        public string Nombre { get; set; }
    }
}
