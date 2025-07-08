using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.LaboratoryTests;

namespace PatientManager.Controllers
{
    public class LaboratoryTestController : Controller
    {
        private readonly ILaboratoryTestService _laboratoryTestService;
        private readonly IUserSessionService _userSessionService;

        public LaboratoryTestController(ILaboratoryTestService laboratoryTestService, IUserSessionService userSessionService)
        {
            _laboratoryTestService = laboratoryTestService;
            _userSessionService = userSessionService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _laboratoryTestService.GetAllAsync());
        }

        public IActionResult Create()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(new LaboratoryTestSaveViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(LaboratoryTestSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            await _laboratoryTestService.AddAsync(saveViewModel, _userSessionService.GetUserInSession("admin").ConsultoryId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _laboratoryTestService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LaboratoryTestSaveViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            await _laboratoryTestService.UpdateAsync(saveViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _laboratoryTestService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            await _laboratoryTestService.RemoveAsync(id);
            return RedirectToAction("Index");
        }
    }
}
