using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.MantenimientoUsuario;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class MantenimientoUsuarioController : Controller
    {
        private readonly IMantenimientoUsuarioService _usuarioService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;

        public MantenimientoUsuarioController(IMantenimientoUsuarioService usuarioService, ValidateUserSession validateUserSession, ValidateUserRole validateUserRole)
        {
            _usuarioService = usuarioService;
            _validateUserSession = validateUserSession;
            _validateUserRole = validateUserRole;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!_validateUserRole.IsAdmin())
            {
                return View("AccesoDenegado");
            }

            var usuarios = await _usuarioService.GetAllViewModel();
            return View(usuarios);
        }

        public IActionResult CrearUsuario()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            return View("SaveMantenimientoUsuario", new SaveMantenimientoUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario(SaveMantenimientoUsuarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMantenimientoUsuario", vm);
            }

            if (await _usuarioService.ExistsByNombreUsuario(vm.NombreUsuario))
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso.");
            }

            if (await _usuarioService.ExistsByCorreo(vm.Correo))
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
            }

            if (vm.Contraseña != vm.ConfirmarContraseña)
            {
                ModelState.AddModelError("ConfirmarContraseña", "Las contraseñas no coinciden.");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMantenimientoUsuario", vm);
            }

            await _usuarioService.Add(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditarUsuario(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var usuario = await _usuarioService.GetByIdSaveViewModel(id);
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }

            return View("SaveMantenimientoUsuario", usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(SaveMantenimientoUsuarioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMantenimientoUsuario", vm);
            }

            var usuarioExistente = await _usuarioService.GetByIdSaveViewModel(vm.Id);
            if (usuarioExistente == null)
            {
                return RedirectToAction("Index");
            }

            if (vm.NombreUsuario != usuarioExistente.NombreUsuario && await _usuarioService.ExistsByNombreUsuario(vm.NombreUsuario))
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso.");
            }

            if (vm.Correo != usuarioExistente.Correo && await _usuarioService.ExistsByCorreo(vm.Correo))
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
            }

            if (!string.IsNullOrEmpty(vm.Contraseña) && vm.Contraseña != vm.ConfirmarContraseña)
            {
                ModelState.AddModelError("ConfirmarContraseña", "Las contraseñas no coinciden.");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMantenimientoUsuario", vm);
            }

            await _usuarioService.Update(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EliminarUsuario(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var usuario = await _usuarioService.GetByIdSaveViewModel(id);
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }

            return View("Delete", usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            try
            {
                await _usuarioService.Delete(id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }

}
