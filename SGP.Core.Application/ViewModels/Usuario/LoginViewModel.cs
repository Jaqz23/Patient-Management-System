using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.Usuario
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
    }
}
