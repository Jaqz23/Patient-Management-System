using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Application.Helpers;
using Sistema_gestor_de_pacientes.Middlewares;
using SGP.Core.Application.ViewModels.Consultorio;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConsultorioService _consultorioService;
        private readonly ValidateUserSession _validateUserSession;

        public UsuarioController(IUsuarioService usuarioService, IConsultorioService consultorioService, ValidateUserSession validateUserSession)
        {
            _usuarioService = usuarioService;
            _consultorioService = consultorioService;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            if (_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Home");
            }

            UsuarioViewModel usuarioVm = await _usuarioService.Login(loginVm);

            if (usuarioVm != null)
            {
                HttpContext.Session.Set<UsuarioViewModel>("usuario", usuarioVm);
                return RedirectToRoute(new {Controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("usuarioValidation", "Usuario o contraseña incorrectos.");
            }
            
            return View(loginVm);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Index");
        }


        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return View(new SaveUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUsuarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            // Validar si el usuario y el correo ya existe
            bool usuarioExistente = await _usuarioService.ExistsByNombreUsuario(vm.NombreUsuario);
            bool correoExistente = await _usuarioService.ExistsByCorreo(vm.Correo);

            if (usuarioExistente)
            {
                ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso.");
            }

            if (correoExistente)
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            ConsultorioViewModel consultorioExistente = await _consultorioService.GetByNombre(vm.ConsultorioNombre);
            int consultorioId;

            if (consultorioExistente != null)
            {
                consultorioId = consultorioExistente.Id;
            }
            else
            {
                
                var nuevoConsultorio = new SaveConsultorioViewModel
                {
                    Nombre = vm.ConsultorioNombre
                };

                SaveConsultorioViewModel consultorioCreado = await _consultorioService.Add(nuevoConsultorio);
                consultorioId = consultorioCreado.Id; 
            }

            
            vm.ConsultorioId = consultorioId;
            await _usuarioService.Add(vm);

            return RedirectToRoute(new { controller = "Usuario", action = "Index" });
        }
    }

}
