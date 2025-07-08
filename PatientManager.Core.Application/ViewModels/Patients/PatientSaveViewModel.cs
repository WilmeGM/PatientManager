using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.Patients
{
    public class PatientSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "ID card is required")]
        [DataType(DataType.Text)]
        public string IdCard { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "This option is required")]
        public bool IsSmoker { get; set; }

        [Required(ErrorMessage = "Allergies are required")]
        [DataType(DataType.Text)]
        public string Allergies { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
