using PatientManager.Core.Application.Helpers;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace PatientManager.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserSessionService _userSessionService;

        public UserController(IUserService userService, IUserSessionService userSessionService)
        {
            _userService = userService;
            _userSessionService = userSessionService;
        }

        public IActionResult Index()
        {
            if (_userSessionService.IsUserLoggedIn("admin") || _userSessionService.IsUserLoggedIn("assis"))
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (_userSessionService.IsUserLoggedIn("admin") || _userSessionService.IsUserLoggedIn("assis"))
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            if (!ModelState.IsValid) return View("Index", loginViewModel);

            var userViewModel = await _userService.LoginAsync(loginViewModel);
            if (userViewModel is not null)
            {
                if (userViewModel.UserType == "Admin")
                {
                    HttpContext.Session.Set<UserViewModel>("admin", userViewModel);
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                } 
                else
                {
                    HttpContext.Session.Set<UserViewModel>("assis", userViewModel);
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
            }
            else
            {
                ModelState.AddModelError("userValidation", "Incorrect login details!");
            }

            return View("Index", loginViewModel);
        }

        public IActionResult Register()
        {
            if (_userSessionService.IsUserLoggedIn("admin") || _userSessionService.IsUserLoggedIn("assis"))
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            return View(new SaveUserForRegister());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserForRegister saveUserForRegister)
        {
            if (_userSessionService.IsUserLoggedIn("admin") || _userSessionService.IsUserLoggedIn("assis"))
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            if (!ModelState.IsValid)
                return View("Register", saveUserForRegister);

            try
            {
                await _userService.AddUserForRegisterAsync(saveUserForRegister);
                return RedirectToRoute(new { controller = "User", action = "Index" });
            } 
            catch(Exception e)
            {
                ModelState.AddModelError("Username", e.Message);
                return View("Register", saveUserForRegister);
            }
        }

        public IActionResult Logout()
        {
            var adminUser = _userSessionService.IsUserLoggedIn("admin");
            var assisUser = _userSessionService.IsUserLoggedIn("assis");

            if (adminUser) HttpContext.Session.Remove("admin");
            if (assisUser) HttpContext.Session.Remove("assis");

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
