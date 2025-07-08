using PatientManager.Core.Application.ViewModels.Doctors;
using PatientManager.Core.Application.ViewModels.Patients;
using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.Appointments
{
    public class AppointmentSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please enter the appointment time")]
        [DataType(DataType.Text)]
        public string AppointmentTime { get; set; }

        [Required(ErrorMessage = "Please enter reason")]
        [DataType(DataType.Text)]
        public string Reason { get; set; }

        public string? Status { get; set; }

        public List<PatientForSelectViewModel>? Patients { get; set; }
        public List<DoctorForSelectViewModel>? Doctors { get; set; }
    }
}
