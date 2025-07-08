namespace PatientManager.Core.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdCard { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsSmoker { get; set; }
        public string Allergies { get; set; }
        public string? PhotoUrl { get; set; }

        // Foreign key
        public int ConsultoryId { get; set; }
        public Consultory Consultory { get; set; }

        // Relationships
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<LaboratoryResult> LaboratoryResults { get; set; }
    }
}
