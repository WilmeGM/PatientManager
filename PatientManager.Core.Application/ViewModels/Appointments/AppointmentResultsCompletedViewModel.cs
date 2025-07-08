using PatientManager.Core.Application.ViewModels.LaboratoryResults;

namespace PatientManager.Core.Application.ViewModels.Appointments
{
    public class AppointmentResultsCompletedViewModel
    {
        public int AppointmentId { get; set; }
        public List<LaboratoryResultViewModelForCompleted> LabResults { get; set; }
    }
}
