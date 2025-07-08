using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.LaboratoryTests
{
    public class LaboratoryTestSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}
