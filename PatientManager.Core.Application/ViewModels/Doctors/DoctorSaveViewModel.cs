using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.Doctors
{
    public class DoctorSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "IdCard is required")]
        [DataType(DataType.Text)]
        public string IdCard { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
