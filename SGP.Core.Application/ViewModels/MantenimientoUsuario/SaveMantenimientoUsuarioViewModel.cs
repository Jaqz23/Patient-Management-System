using SGP.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGP.Core.Application.ViewModels.MantenimientoUsuario
{
    public class SaveMantenimientoUsuarioViewModel
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

        [Required(ErrorMessage = "Debe seleccionar un tipo de usuario")]
        public RolUsuario Rol { get; set; }
    }
}
