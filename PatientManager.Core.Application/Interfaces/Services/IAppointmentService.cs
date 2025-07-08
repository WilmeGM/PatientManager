using PatientManager.Core.Application.ViewModels.Appointments;
using PatientManager.Core.Application.ViewModels.Doctors;
using PatientManager.Core.Application.ViewModels.LaboratoryResults;
using PatientManager.Core.Application.ViewModels.Patients;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IAppointmentService : IGenericService<AppointmentSaveViewModel, AppointmentViewModel>
    {
        Task<List<PatientForSelectViewModel>> GetAllPatientsAsync();
        Task<List<DoctorForSelectViewModel>> GetAllDoctorsAsync();
        Task MarkAppointmentAsPendingResults(int appointmentId, List<int> labTestIds);
        Task MarkAppointmentAsCompleted(int appointmentId);
        Task<List<LaboratoryResultViewModelForPendingResults>> GetLabResultsForAppointment(int appointmentId);
        Task<List<LaboratoryResultViewModelForCompleted>> GetLabResultsCompletedForAppointment(int appointmentId);
    }
}
