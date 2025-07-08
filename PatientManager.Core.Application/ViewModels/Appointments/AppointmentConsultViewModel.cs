using PatientManager.Core.Application.ViewModels.LaboratoryTests;

namespace PatientManager.Core.Application.ViewModels.Appointments
{
    public class AppointmentConsultViewModel
    {
        public int AppointmentId { get; set; }
        public List<LaboratoryTestViewModel> LabTests { get; set; } 
        public List<int> SelectedLabTests { get; set; } 
    }
}
