using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.PruebaLaboratorio
{
    public class SavePruebaLaboratorioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de la prueba")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        public int ConsultorioId { get; set; }
    }
}
