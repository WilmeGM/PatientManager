using System.ComponentModel.DataAnnotations;

namespace PatientManager.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter username")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
