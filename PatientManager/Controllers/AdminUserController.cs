using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Users;

namespace PatientManager.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserSessionService _userSessionService;

        public AdminUserController(IUserService userService, IUserSessionService userSessionService)
        {
            _userService = userService;
            _userSessionService = userSessionService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _userService.GetAllAsync());
        }

        public IActionResult Create()
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel saveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveViewModel);

            try
            {
                await _userService.AddAsync(saveViewModel, _userSessionService.GetUserInSession("admin").ConsultoryId);
                return RedirectToRoute(new { controller = "AdminUser", action = "Index" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Username", e.Message);
                return View(saveViewModel);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _userService.GetByIdForEditionAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserForEdition saveUserForEdition)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (!ModelState.IsValid) return View(saveUserForEdition);

            try
            {
                await _userService.UpdateAsync(saveUserForEdition);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Username", e.Message);
                return View(saveUserForEdition);
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _userService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("admin")) return RedirectToRoute(new { controller = "User", action = "Index" });

            await _userService.RemoveAsync(id);
            return RedirectToAction("Index");
        }
    }
}
