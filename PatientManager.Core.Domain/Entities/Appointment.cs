namespace PatientManager.Core.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // "Pending consultation", "Pending results", "Completed"

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int ConsultoryId { get; set; }
        public Consultory Consultory { get; set; }

        public ICollection<LaboratoryResult> LaboratoryResults { get; set; }
    }
}
