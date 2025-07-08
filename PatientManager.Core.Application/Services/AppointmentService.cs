using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Appointments;
using PatientManager.Core.Application.ViewModels.Doctors;
using PatientManager.Core.Application.ViewModels.LaboratoryResults;
using PatientManager.Core.Application.ViewModels.Patients;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ILaboratoryResultRepository _laboratoryResultRepository;
        private readonly IUserSessionService _userSessionService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IUserSessionService userSessionService,
            ILaboratoryResultRepository laboratoryResultRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _userSessionService = userSessionService;
            _laboratoryResultRepository = laboratoryResultRepository;
        }

        public async Task<List<AppointmentViewModel>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllWithIncludeAsync(new List<string> { "Patient", "Doctor" });

            return appointments.Where(a => a.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId).Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                DoctorName = $"{a.Doctor.FirstName} {a.Doctor.LastName}",
                AppointmentDate = a.Date,
                AppointmentTime = a.Time,
                Reason = a.Reason,
                Status = a.Status
            }).ToList();
        }

        public async Task<AppointmentSaveViewModel> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            return new AppointmentSaveViewModel
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.Date,
                AppointmentTime = appointment.Time,
                Reason = appointment.Reason,
                Status = appointment.Status
            };
        }

        public async Task<AppointmentSaveViewModel> AddAsync(AppointmentSaveViewModel saveViewModel, int consultoryId)
        {
            var appointment = new Appointment
            {
                PatientId = saveViewModel.PatientId,
                DoctorId = saveViewModel.DoctorId,
                Date = saveViewModel.AppointmentDate,
                Time = saveViewModel.AppointmentTime,
                Reason = saveViewModel.Reason,
                Status = "Pending consultation",
                ConsultoryId = consultoryId
            };

            appointment = await _appointmentRepository.AddAsync(appointment);

            saveViewModel.Id = appointment.Id;
            return saveViewModel;
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                await _appointmentRepository.RemoveAsync(await _appointmentRepository.GetByIdAsync(id));
            } 
            catch(Exception)
            {
                throw new Exception("Cannot delete this appointment because has lab results.");
            }
        }

        public async Task<List<PatientForSelectViewModel>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            return patients.Where(a => a.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId).Select(p => new PatientForSelectViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName
            }).ToList();
        }

        public async Task<List<DoctorForSelectViewModel>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return doctors.Where(d => d.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId).Select(d => new DoctorForSelectViewModel
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName
            }).ToList();
        }

        public async Task MarkAppointmentAsPendingResults(int appointmentId, List<int> labTestIds)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment != null && appointment.Status == "Pending consultation")
            {
                appointment.Status = "Pending results";

                foreach (var labTestId in labTestIds)
                {
                    var labResult = new LaboratoryResult
                    {
                        ConsultoryId = appointment.ConsultoryId,
                        LaboratoryTestId = labTestId,
                        AppointmentId = appointmentId,
                        PatientId = appointment.PatientId,
                        IsCompleted = false
                    };

                    await _laboratoryResultRepository.AddAsync(labResult);
                }

                await _appointmentRepository.UpdateAsync(appointment);
            }
        }

        public async Task MarkAppointmentAsCompleted(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment != null && appointment.Status == "Pending results")
            {
                appointment.Status = "Completed";
                await _appointmentRepository.UpdateAsync(appointment);
            }
        }

        public async Task<List<LaboratoryResultViewModelForPendingResults>> GetLabResultsForAppointment(int appointmentId)
        {
            var labResults = await _laboratoryResultRepository.GetAllWithIncludeAsync(new List<string> { "LaboratoryTest" });

            return labResults.Where(lr => lr.AppointmentId == appointmentId).Select(lr => new LaboratoryResultViewModelForPendingResults
            {
                Name = lr.LaboratoryTest.Name,
                IsCompleted = lr.IsCompleted
            }).ToList();
        }

        public async Task<List<LaboratoryResultViewModelForCompleted>> GetLabResultsCompletedForAppointment(int appointmentId)
        {
            var labResults = await _laboratoryResultRepository.GetAllWithIncludeAsync(new List<string> { "LaboratoryTest" });

            return labResults.Where(lr => lr.AppointmentId == appointmentId && lr.IsCompleted == true).Select(lr => new LaboratoryResultViewModelForCompleted
            {
                Name = lr.LaboratoryTest.Name,
                Result = lr.Result
            }).ToList();
        }
    }
}
