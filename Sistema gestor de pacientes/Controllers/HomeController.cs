using Microsoft.AspNetCore.Mvc;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(ILogger<HomeController> logger, ValidateUserSession validateUserSession)
        {
            _logger = logger;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (!_validateUserSession.HasUser()) 
            {
                return RedirectToRoute(new { controller = "Usuario", action = "Index" });
            }
            return View();
        }

    }
}
