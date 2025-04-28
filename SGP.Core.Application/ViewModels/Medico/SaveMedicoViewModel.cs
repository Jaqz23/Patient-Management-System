using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.Medico
{
    public class SaveMedicoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar un correo electrónico")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cédula")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La cédula solo debe contener números y tener 11 digitos")]
        public string Cedula { get; set; }


        [Required(ErrorMessage = "Debe ingresar un teléfono")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El numero de teléfono debe tener 10 digitos")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public string? Foto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un consultorio")]
        public int ConsultorioId { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
}
