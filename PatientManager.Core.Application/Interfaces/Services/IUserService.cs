using PatientManager.Core.Application.ViewModels.Users;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<SaveUserViewModel, UserViewModel>, IUpdateService<SaveUserForEdition>
    {
        Task<UserViewModel> LoginAsync(LoginViewModel loginViewModel);
        Task<SaveUserForRegister> AddUserForRegisterAsync(SaveUserForRegister saveUserForRegister);
        Task<SaveUserForEdition> GetByIdForEditionAsync(int id);
    }
}
