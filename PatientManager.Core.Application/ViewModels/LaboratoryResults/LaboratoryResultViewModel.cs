namespace PatientManager.Core.Application.ViewModels.LaboratoryResults
{
    public class LaboratoryResultViewModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PatientIdCard { get; set; }
        public string TestName { get; set; }
        public string? Result { get; set; }
        public bool IsCompleted { get; set; }
    }
}
