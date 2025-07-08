using PatientManager.Core.Application.ViewModels.Users;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IUserSessionService
    {
        UserViewModel GetUserInSession(string typeUser);
        bool IsUserLoggedIn(string typeUser);
    }
}
