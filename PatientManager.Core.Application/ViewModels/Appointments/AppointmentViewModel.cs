namespace PatientManager.Core.Application.ViewModels.Appointments
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
