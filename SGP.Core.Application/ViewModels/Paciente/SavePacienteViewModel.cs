using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.Paciente
{
    public class SavePacienteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar un teléfono")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El numero de teléfono debe tener 10 digitos")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar una dirección")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cédula")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La cédula solo debe contener números y tener 11 digitos")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar si el paciente es fumador o no")]
        public bool EsFumador { get; set; }

        [Required(ErrorMessage = "Debe especificar si el paciente tiene alergias")]
        [DataType(DataType.MultilineText)]
        public string Alergias { get; set; }

        public string? Foto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un consultorio")]
        public int ConsultorioId { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
}
