using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Patients;

namespace PatientManager.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IUserSessionService _userSessionService;

        public PatientController(IPatientService patientService, IUserSessionService userSessionService)
        {
            _patientService = patientService;
            _userSessionService = userSessionService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _patientService.GetAllAsync());
        }

        public IActionResult Create()
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(new PatientSaveViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            var savedPatient = await _patientService.AddAsync(saveViewModel, _userSessionService.GetUserInSession("assis").ConsultoryId);

            if (savedPatient.Id != 0 && saveViewModel.Photo != null)
            {
                savedPatient.PhotoUrl = UploadFile(saveViewModel.Photo, savedPatient.Id);
                await _patientService.UpdateAsync(savedPatient);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _patientService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PatientSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            var existingPatient = await _patientService.GetByIdAsync(saveViewModel.Id);
            saveViewModel.PhotoUrl = UploadFile(saveViewModel.Photo, saveViewModel.Id, true, existingPatient.PhotoUrl);

            await _patientService.UpdateAsync(saveViewModel);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _patientService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            try
            {
                await _patientService.RemoveAsync(id);
                DeletePatientPhoto(id);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Remove", await _patientService.GetByIdAsync(id));
            }
        }

        private string UploadFile(IFormFile file, int id, bool editMode = false, string imageUrl = "")
        {
            if (editMode && file == null) return imageUrl;

            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string basePath = $"/Images/Patients/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            string pathWithFileName = Path.Combine(path, fileName);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            using (var stream = new FileStream(pathWithFileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (editMode)
            {
                string[] oldImagePart = imageUrl.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }

        private void DeletePatientPhoto(int id)
        {
            string basePath = $"/Images/Patients/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                var directoryInfo = new DirectoryInfo(path);

                foreach (var file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (var directory in directoryInfo.GetDirectories())
                {
                    directory.Delete(true);
                }

                Directory.Delete(path);
            }
        }
    }
}
