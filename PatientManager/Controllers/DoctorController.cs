using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Doctors;

namespace PatientManager.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IUserSessionService _userSessionService;

        public DoctorController(IDoctorService doctorService, IUserSessionService userSessionService)
        {
            _doctorService = doctorService;
            _userSessionService = userSessionService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _doctorService.GetAllAsync());
        }

        public IActionResult Create()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(new DoctorSaveViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            var savedDoctor = await _doctorService.AddAsync(saveViewModel, _userSessionService.GetUserInSession("admin").ConsultoryId);

            if (savedDoctor.Id != 0 && saveViewModel.Photo != null)
            {
                savedDoctor.PhotoUrl = UploadFile(saveViewModel.Photo, savedDoctor.Id);
                await _doctorService.UpdateAsync(savedDoctor);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _doctorService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            var existingDoctor = await _doctorService.GetByIdAsync(saveViewModel.Id);
            saveViewModel.PhotoUrl = UploadFile(saveViewModel.Photo, saveViewModel.Id, true, existingDoctor.PhotoUrl);
            await _doctorService.UpdateAsync(saveViewModel);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _doctorService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            try
            {
                await _doctorService.RemoveAsync(id);
                DeleteDoctorPhoto(id);
                return RedirectToAction("Index");
            } 
            catch(Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Remove", await _doctorService.GetByIdAsync(id));
            }
        }

        private string UploadFile(IFormFile file, int id, bool editMode = false, string imageUrl = "")
        {
            if (editMode && file == null) return imageUrl;

            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string basePath = $"/Images/Doctors/{id}";
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

        private void DeleteDoctorPhoto(int id)
        {
            string basePath = $"/Images/Doctors/{id}";
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
