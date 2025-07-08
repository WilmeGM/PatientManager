namespace PatientManager.Core.Domain.Entities
{
    public class LaboratoryResult
    {
        public int Id { get; set; }
        public string? Result { get; set; }
        public bool IsCompleted { get; set; }
         
        // Foreign keys
        public int LaboratoryTestId { get; set; }
        public LaboratoryTest LaboratoryTest { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public int ConsultoryId { get; set; }
        public Consultory Consultory { get; set; }
    }
}
