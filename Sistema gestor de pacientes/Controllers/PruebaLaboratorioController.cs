using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.PruebaLaboratorio;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class PruebaLaboratorioController : Controller
    {
        private readonly IPruebaLaboratorioService _pruebaLaboratorioService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;

        public PruebaLaboratorioController(IPruebaLaboratorioService pruebaLaboratorioService, ValidateUserSession validateUserSession, ValidateUserRole validateUserRole)
        {
            _pruebaLaboratorioService = pruebaLaboratorioService;
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

            var pruebas = await _pruebaLaboratorioService.GetAllViewModel();
            return View(pruebas);
        }

        
        public IActionResult Crear()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            return View("SavePruebaLaboratorio", new SavePruebaLaboratorioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SavePruebaLaboratorioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid) 
            {
                return View("SavePruebaLaboratorio", vm);
            }

            await _pruebaLaboratorioService.Add(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var prueba = await _pruebaLaboratorioService.GetByIdSaveViewModel(id);
            if (prueba == null)
            {
                return RedirectToAction("Index");
            }

            return View("SavePruebaLaboratorio", prueba);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SavePruebaLaboratorioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePruebaLaboratorio", vm);
            }

            await _pruebaLaboratorioService.Update(vm);
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var prueba = await _pruebaLaboratorioService.GetByIdSaveViewModel(id);
            if (prueba == null)
            {
                return RedirectToAction("Index");
            }

            return View("Delete", prueba);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var prueba = await _pruebaLaboratorioService.GetByIdSaveViewModel(id);
            if (prueba == null)
            {
                return RedirectToAction("Index");
            }

            await _pruebaLaboratorioService.Delete(id);
            return RedirectToAction("Index");
        }
    }

}
