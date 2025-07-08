namespace PatientManager.Core.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public int ConsultoryId { get; set; }
    }
}
