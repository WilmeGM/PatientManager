using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.LaboratoryResults;

namespace PatientManager.Controllers
{
    public class LaboratoryResultController : Controller
    {
        private readonly ILaboratoryResultService _laboratoryResultService;
        private readonly IUserSessionService _userSessionService;

        public LaboratoryResultController(ILaboratoryResultService laboratoryResultService, IUserSessionService userSessionService)
        {
            _laboratoryResultService = laboratoryResultService;
            _userSessionService = userSessionService;
        }

        public async Task<IActionResult> Index(string? patientIdCard)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _laboratoryResultService.GetAllPendingResultsAsync(patientIdCard));
        }

        public async Task<IActionResult> Report(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _laboratoryResultService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Report(LaboratoryResultViewModel viewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (string.IsNullOrEmpty(viewModel.Result))
            {
                ModelState.AddModelError("", "Please provide the result.");
                return View(viewModel);
            }

            await _laboratoryResultService.ReportResultAsync(viewModel.Id, viewModel.Result);
            return RedirectToAction("Index");
        }
    }
}
