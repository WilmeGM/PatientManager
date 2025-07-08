using PatientManager.Core.Application.ViewModels.LaboratoryResults;

namespace PatientManager.Core.Application.ViewModels.Appointments
{
    public class AppointmentResultsViewModel
    {
        public int AppointmentId { get; set; } 
        public List<LaboratoryResultViewModelForPendingResults> LabResults { get; set; } 
    }
}
