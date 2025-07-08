namespace PatientManager.Core.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public string? PhotoUrl { get; set; }
        public int ConsultoryId { get; set; }
        public Consultory Consultory { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
