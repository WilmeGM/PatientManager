using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.Users
{
    public class SaveUserForRegister
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

        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Consultory name is required")]
        [DataType(DataType.Text)]
        public string ConsultoryName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public string UserType { get; set; }
    }
}
