using PatientManager.Core.Application.ViewModels.Users;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(LoginViewModel loginViewModel);
        Task<bool> UsernameExists(string username);
        Task<User> GetByUsernameAsync(string username);
    }
}
