using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.Usuario
{
    public class SaveUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string NombreUsuario { get; set; }

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

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Compare(nameof(Contraseña), ErrorMessage = "Las contraseñas no coinciden")]
        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmarContraseña { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del consultorio")]
        public string ConsultorioNombre { get; set; }

        public int ConsultorioId { get; set; } 
    }
}
