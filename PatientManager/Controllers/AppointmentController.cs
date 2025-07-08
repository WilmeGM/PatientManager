using Microsoft.AspNetCore.Mvc;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Appointments;

namespace PatientManager.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILaboratoryTestService _laboratoryTestService;
        private readonly IUserSessionService _userSessionService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IUserSessionService userSessionService,
            ILaboratoryTestService laboratoryTestService)
        {
            _appointmentService = appointmentService;
            _userSessionService = userSessionService;
            _laboratoryTestService = laboratoryTestService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _appointmentService.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            var appointmentSaveViewModel = new AppointmentSaveViewModel
            {
                Patients = await _appointmentService.GetAllPatientsAsync(),
                Doctors = await _appointmentService.GetAllDoctorsAsync()
            };

            return View(appointmentSaveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentSaveViewModel appointmentSaveViewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            appointmentSaveViewModel.Patients = await _appointmentService.GetAllPatientsAsync();
            appointmentSaveViewModel.Doctors = await _appointmentService.GetAllDoctorsAsync();

            if (!ModelState.IsValid) return View("Create", appointmentSaveViewModel);

            await _appointmentService.AddAsync(appointmentSaveViewModel, _userSessionService.GetUserInSession("assis").ConsultoryId);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            return View(await _appointmentService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            try
            {
                await _appointmentService.RemoveAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Remove", await _appointmentService.GetByIdAsync(id));
            }
        }

        public async Task<IActionResult> Consult(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null || appointment.Status != "Pending consultation") return RedirectToAction("Index");

            var labTests = await _laboratoryTestService.GetAllAsyncForAssistant();
            var viewModel = new AppointmentConsultViewModel
            {
                AppointmentId = id,
                LabTests = labTests
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PerformTests(AppointmentConsultViewModel viewModel)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            if (viewModel.SelectedLabTests == null || !viewModel.SelectedLabTests.Any())
            {
                ModelState.AddModelError("", "Please select at least one lab test.");
                return View("Consult", viewModel);
            }

            await _appointmentService.MarkAppointmentAsPendingResults(viewModel.AppointmentId, viewModel.SelectedLabTests);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConsultResults(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null || appointment.Status != "Pending results") return RedirectToAction("Index");

            var labResults = await _appointmentService.GetLabResultsForAppointment(id);
            var viewModel = new AppointmentResultsViewModel
            {
                AppointmentId = id,
                LabResults = labResults
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAppointment(int AppointmentId)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            await _appointmentService.MarkAppointmentAsCompleted(AppointmentId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewResults(int id)
        {
            if (!_userSessionService.IsUserLoggedIn("assis")) return RedirectToRoute(new { controller = "User", action = "Index" });

            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null || appointment.Status != "Completed") return RedirectToAction("Index");

            var labResults = await _appointmentService.GetLabResultsCompletedForAppointment(id);
            var viewModel = new AppointmentResultsCompletedViewModel
            {
                AppointmentId = id,
                LabResults = labResults
            };

            return View(viewModel);
        }
    }
}
