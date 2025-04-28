using SGP.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.ResultadoLaboratorio;
using SGP.Core.Application.ViewModels.Usuario;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class ResultadoLaboratorioController : Controller
    {
        private readonly IResultadoLaboratorioService _resultadoService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;
        private readonly UsuarioViewModel _usuarioActual;

        public ResultadoLaboratorioController(
            IResultadoLaboratorioService resultadoService,
            ValidateUserSession validateUserSession, 
            ValidateUserRole validateUserRole,
            IHttpContextAccessor httpContextAccessor)
        {
            _resultadoService = resultadoService;
            _validateUserSession = validateUserSession;
            _validateUserRole = validateUserRole;
            _usuarioActual = httpContextAccessor.HttpContext.Session.Get<UsuarioViewModel>("usuario");
        }

        
        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser() || _usuarioActual == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!_validateUserRole.IsAsistente())
            {
                return View("AccesoDenegado");
            }

            var resultados = await _resultadoService.GetResultadosPendientesByConsultorioAsync(_usuarioActual.ConsultorioId);
            return View(resultados);
        }

        
        [HttpPost]
        public async Task<IActionResult> FiltrarPorCedula(string cedula)
        {
            if (!_validateUserSession.HasUser() || _usuarioActual == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (string.IsNullOrEmpty(cedula))
            {
                return RedirectToAction("Index");
            }

            var resultados = await _resultadoService.GetResultadosByCedulaAsync(_usuarioActual.ConsultorioId, cedula);
            return View("Index", resultados);
        }

        
        public async Task<IActionResult> Reportar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var resultado = await _resultadoService.GetByIdSaveViewModel(id);
            if (resultado == null)
            {
                return RedirectToAction("Index");
            }

            return View("ReportarResultado", resultado);
        }

        
        [HttpPost]
        public async Task<IActionResult> Reportar(SaveResultadoLaboratorioViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (string.IsNullOrEmpty(vm.Resultado))
            {
                ModelState.AddModelError("Resultado", "Debe ingresar un resultado.");
                return View("ReportarResultado", vm);
            }

            await _resultadoService.ReportarResultado(vm.Id, vm.Resultado);
            return RedirectToAction("Index");
        }
    }
}

