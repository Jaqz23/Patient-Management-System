using SGP.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Cita;
using SGP.Core.Application.ViewModels.Usuario;
using SGP.Core.Domain.Enums;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IPruebaLaboratorioService _pruebaLaboratorioService;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;
        private readonly UsuarioViewModel _usuarioActual;

        public CitaController(ICitaService citaService,
                              IPruebaLaboratorioService pruebaLaboratorioService,
                              IPacienteService pacienteService,
                              IMedicoService medicoService,
                              ValidateUserSession validateUserSession,
                              ValidateUserRole validateUserRole,
                              IHttpContextAccessor httpContextAccessor)
        {
            _citaService = citaService;
            _pruebaLaboratorioService = pruebaLaboratorioService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
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

            var citas = await _citaService.GetAllViewModel();
            return View(citas);
        }

 
        private async Task CargarListas()
        {
            if (_usuarioActual != null)
            {
                ViewBag.Pacientes = await _pacienteService.GetPacientesByConsultorioAsync(_usuarioActual.ConsultorioId);
                ViewBag.Medicos = await _medicoService.GetMedicosByConsultorioAsync(_usuarioActual.ConsultorioId);
            }
        }

        
        public async Task<IActionResult> Crear()
        {
            if (!_validateUserSession.HasUser() || _usuarioActual == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            await CargarListas();
            return View("SaveCita", new SaveCitaViewModel());
        }

        
        [HttpPost]
        public async Task<IActionResult> Crear(SaveCitaViewModel vm)
        {
            if (!_validateUserSession.HasUser() || _usuarioActual == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                await CargarListas();
                return View("SaveCita", vm);
            }

            await _citaService.Add(vm);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Eliminar(int id)
        {
            if (!_validateUserSession.HasUser()) return RedirectToAction("Index", "Usuario");

            var cita = await _citaService.GetByIdSaveViewModel(id);
            if (cita == null) return RedirectToAction("Index");

            return View("Delete", cita);
        }

        
        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            if (!_validateUserSession.HasUser()) return RedirectToAction("Index", "Usuario");

            await _citaService.Delete(id);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Consultar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var cita = await _citaService.GetByIdSaveViewModel(id);
            if (cita == null)
            {
                return RedirectToAction("Index");
            }

            var pruebas = await _pruebaLaboratorioService.GetAllViewModel();
            ViewBag.Pruebas = pruebas;
            return View("Consultar", cita);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarPruebas(int citaId, List<int> pruebaIds)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (pruebaIds == null || pruebaIds.Count == 0)
            {
                return RedirectToAction("Consultar", new { id = citaId });
            }

            await _citaService.AsignarPruebasALaCita(citaId, pruebaIds);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ConsultarResultados(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var resultados = await _citaService.GetResultadosPorCita(id);
            if (resultados == null || resultados.Count == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CitaId = id;
            return View("ConsultarResultados", resultados);
        }

        
        public async Task<IActionResult> CompletarCita(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            await _citaService.CambiarEstadoCita(id, EstadoCita.Completada);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerResultados(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var resultados = await _citaService.GetResultadosCompletadosPorCita(id);
            if (resultados == null || resultados.Count == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CitaId = id;
            return View("VerResultados", resultados);
        }


        public IActionResult CerrarResultados()
        {
            return RedirectToAction("Index");
        }

    }
}
