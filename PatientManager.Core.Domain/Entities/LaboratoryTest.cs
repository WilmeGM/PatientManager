namespace PatientManager.Core.Domain.Entities
{
    public class LaboratoryTest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key
        public int ConsultoryId { get; set; }
        public Consultory Consultory { get; set; }

        // Relationships
        public ICollection<LaboratoryResult> LaboratoryResults { get; set; }
    }
}
