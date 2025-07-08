using System.Numerics;

namespace PatientManager.Core.Domain.Entities
{
    public class Consultory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<LaboratoryTest> LaboratoryTests { get; set; }
        public ICollection<LaboratoryResult> LaboratoryResults { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
