using Microsoft.AspNetCore.Http;
using PatientManager.Core.Application.Helpers;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Users;

namespace PatientManager.Core.Application.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserSessionService(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        public UserViewModel GetUserInSession(string typeUser) => _contextAccessor.HttpContext.Session.Get<UserViewModel>(typeUser);

        public bool IsUserLoggedIn(string typeUser)
        {
            if (_contextAccessor.HttpContext.Session.Get<UserViewModel>(typeUser) is null) return false;

            return true;
        }
    }
}
