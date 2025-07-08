namespace PatientManager.Core.Application.ViewModels.Doctors
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
