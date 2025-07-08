using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Users;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConsultoryRepository _consultoryRepository;
        private readonly IUserSessionService _userSessionService;

        public UserService(IUserRepository userRepository, IConsultoryRepository consultoryRepository, IUserSessionService userSessionService)
        {
            _userRepository = userRepository;
            _consultoryRepository = consultoryRepository;
            _userSessionService = userSessionService;
        }

        public async Task<UserViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userRepository.LoginAsync(loginViewModel);

            if (user is null) return null;

            return new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType,
                ConsultoryId = user.ConsultoryId
            };
        }

        private async Task<Consultory> AddConsultoryAsync(string consultoryName)
        {
            var consultory = new Consultory
            {
                Name = consultoryName
            };

            consultory = await _consultoryRepository.AddAsync(consultory);
            return consultory;
        } 

        public async Task<SaveUserForRegister> AddUserForRegisterAsync(SaveUserForRegister saveUserForRegister)
        {
            if (await _userRepository.UsernameExists(saveUserForRegister.Username)) throw new Exception("Username already exists. Please choose another one.");

            var consultory = await AddConsultoryAsync(saveUserForRegister.ConsultoryName);

            var user = new User
            {
                Username = saveUserForRegister.Username,
                Password = saveUserForRegister.Password,
                FirstName = saveUserForRegister.Firstname,
                LastName = saveUserForRegister.Lastname,
                Email = saveUserForRegister.Email,
                UserType = saveUserForRegister.UserType,
                ConsultoryId = consultory.Id
            };

            user = await _userRepository.AddAsync(user);

            saveUserForRegister.Id = user.Id;
            return saveUserForRegister;
        }

        public async Task<SaveUserViewModel> AddAsync(SaveUserViewModel saveViewModel, int consultoryId)
        {
            if (await _userRepository.UsernameExists(saveViewModel.Username)) throw new Exception("Username already exists. Please choose another one.");

            var user = new User
            {
                Username = saveViewModel.Username,
                Password = saveViewModel.Password,
                FirstName = saveViewModel.Firstname,
                LastName = saveViewModel.Lastname,
                Email = saveViewModel.Email,
                UserType = saveViewModel.UserType,
                ConsultoryId = consultoryId
            };

            user = await _userRepository.AddAsync(user);
            saveViewModel.Id = user.Id;

            return saveViewModel;
        }


        public async Task UpdateAsync(SaveUserForEdition saveUserForEdition)
        {
            var existingUser = await _userRepository.GetByIdAsync(saveUserForEdition.Id);

            var userWithSameUsername = await _userRepository.GetByUsernameAsync(saveUserForEdition.Username);
            if (userWithSameUsername != null && userWithSameUsername.Id != saveUserForEdition.Id) throw new Exception("Username already exists. Please choose another one.");

            existingUser.Id = saveUserForEdition.Id;
            existingUser.Username = saveUserForEdition.Username;
            if (!string.IsNullOrWhiteSpace(saveUserForEdition.Password)) existingUser.Password = saveUserForEdition.Password;
            existingUser.FirstName = saveUserForEdition.Firstname;
            existingUser.LastName = saveUserForEdition.Lastname;
            existingUser.Email = saveUserForEdition.Email;
            existingUser.UserType = saveUserForEdition.UserType;

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task RemoveAsync(int id)
        {
            await _userRepository.RemoveAsync(await _userRepository.GetByIdAsync(id));
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var usersList = await _userRepository.GetAllAsync();

            return usersList.Where(user => user.ConsultoryId == _userSessionService.GetUserInSession("admin").ConsultoryId).Select(user => new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType
            }).ToList();
        }

        public async Task<SaveUserViewModel> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return new SaveUserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Password = "",
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email,
                UserType = user.UserType
            };
        }

        public async Task<SaveUserForEdition> GetByIdForEditionAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return new SaveUserForEdition
            {
                Id = user.Id,
                Username = user.Username,
                Password = "",
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email,
                UserType = user.UserType
            };
        }
    }
}
