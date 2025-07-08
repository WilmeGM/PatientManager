using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;

namespace PatientManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserSessionService _userSessionService;

        public HomeController(IUserSessionService userSessionService) => _userSessionService = userSessionService;

        public IActionResult Index()
        {
            return View();
        }
    }
}
