using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Paciente;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteService _pacienteService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;

        public PacienteController(IPacienteService pacienteService, ValidateUserSession validateUserSession, ValidateUserRole validateUserRole)
        {
            _pacienteService = pacienteService;
            _validateUserSession = validateUserSession;
            _validateUserRole = validateUserRole;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!_validateUserRole.IsAsistente())
            {
                return View("AccesoDenegado");
            }

            var pacientes = await _pacienteService.GetAllViewModel();
            return View(pacientes);
        }

        public IActionResult Crear()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            return View("SavePaciente", new SavePacienteViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SavePacienteViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePaciente", vm);
            }

            if (await _pacienteService.ExistsByCedulaAsync(vm.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula ya está registrada.");
            }

            if (await _pacienteService.ExistsByTelefonoAsync(vm.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePaciente", vm);
            }

            SavePacienteViewModel pacienteVm = await _pacienteService.Add(vm);

            if (pacienteVm.Id != 0)
            {
                pacienteVm.Foto = UploadFile(vm.File, pacienteVm.Id);
                await _pacienteService.Update(pacienteVm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var paciente = await _pacienteService.GetByIdSaveViewModel(id);
            if (paciente == null)
            {
                return RedirectToAction("Index");
            }

            return View("SavePaciente", paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SavePacienteViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePaciente", vm);
            }

            var pacienteExistente = await _pacienteService.GetByIdSaveViewModel(vm.Id);
            if (pacienteExistente == null)
            {
                return RedirectToAction("Index");
            }

            if (vm.Cedula != pacienteExistente.Cedula && await _pacienteService.ExistsByCedulaAsync(vm.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula ya está registrada.");
            }

            if (vm.Telefono != pacienteExistente.Telefono && await _pacienteService.ExistsByTelefonoAsync(vm.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                return View("SavePaciente", vm);
            }

            SavePacienteViewModel pacienteVm = await _pacienteService.GetByIdSaveViewModel(vm.Id);
            vm.Foto = UploadFile(vm.File, vm.Id, true, pacienteVm.Foto);
            await _pacienteService.Update(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var paciente = await _pacienteService.GetByIdSaveViewModel(id);
            if (paciente == null)
            {
                return RedirectToAction("Index");
            }

            return View("Delete", paciente);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var paciente = await _pacienteService.GetByIdSaveViewModel(id);
            if (paciente == null)
            {
                return RedirectToAction("Index");
            }

            string basePath = $"/Images/Pacientes/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }
            await _pacienteService.Delete(id);
            return RedirectToAction("Index");
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            string basePath = $"/Images/Pacientes/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }
    }
}
