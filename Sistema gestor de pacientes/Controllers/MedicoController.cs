using Microsoft.AspNetCore.Mvc;
using SGP.Core.Application.Interfaces.Services;
using SGP.Core.Application.ViewModels.Medico;
using Sistema_gestor_de_pacientes.Middlewares;

namespace Sistema_gestor_de_pacientes.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoService _medicoService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly ValidateUserRole _validateUserRole;

        public MedicoController(IMedicoService medicoService, ValidateUserSession validateUserSession, ValidateUserRole validateUserRole)
        {
            _medicoService = medicoService;
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

            var medicos = await _medicoService.GetAllViewModel();
            return View(medicos);
        }

        public IActionResult Crear()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            return View("SaveMedico",new SaveMedicoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SaveMedicoViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMedico", vm);
            }

            if (await _medicoService.ExistsByCorreo(vm.Correo))
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
            }

            if (await _medicoService.ExistsByCedula(vm.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula ya está registrada.");
            }

            if (await _medicoService.ExistsByTelefono(vm.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMedico", vm);
            }

            SaveMedicoViewModel medicoVm = await _medicoService.Add(vm);
            
            if (medicoVm.Id != 0 && medicoVm.Id != null) 
            {
                medicoVm.Foto = UploadFile(vm.File, medicoVm.Id);

                await _medicoService.Update(medicoVm);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var medico = await _medicoService.GetByIdSaveViewModel(id);
            if (medico == null)
            {
                return RedirectToAction("Index");
            }

            return View("SaveMedico", medico);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SaveMedicoViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMedico", vm);
            }

            var medicoExistente = await _medicoService.GetByIdSaveViewModel(vm.Id);
            if (medicoExistente == null)
            {
                return RedirectToAction("Index");
            }

            if (vm.Correo != medicoExistente.Correo && await _medicoService.ExistsByCorreo(vm.Correo))
            {
                ModelState.AddModelError("Correo", "El correo electrónico ya está registrado.");
            }

            if (vm.Cedula != medicoExistente.Cedula && await _medicoService.ExistsByCedula(vm.Cedula))
            {
                ModelState.AddModelError("Cedula", "La cédula ya está registrada.");
            }

            if (vm.Telefono != medicoExistente.Telefono && await _medicoService.ExistsByTelefono(vm.Telefono))
            {
                ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveMedico", vm);
            }

            SaveMedicoViewModel medicoVm = await _medicoService.GetByIdSaveViewModel(vm.Id);
            vm.Foto = UploadFile(vm.File, vm.Id, true, medicoVm.Foto);
            await _medicoService.Update(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var medico = await _medicoService.GetByIdSaveViewModel(id);
            if (medico == null)
            {
                return RedirectToAction("Index");
            }

            return View("Delete", medico);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToAction("Index", "Usuario");
            }

            var medico = await _medicoService.GetByIdSaveViewModel(id);
            if (medico == null)
            {
                return RedirectToAction("Index");
            }

            string basePath = $"/Images/Medicos/{id}";
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
            await _medicoService.Delete(id);
            return RedirectToAction("Index");
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Medicos/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
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
