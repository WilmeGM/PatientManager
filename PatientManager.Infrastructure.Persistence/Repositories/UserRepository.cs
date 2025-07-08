using Microsoft.EntityFrameworkCore;
using PatientManager.Core.Application.Helpers;
using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.ViewModels.Users;
using PatientManager.Core.Domain.Entities;
using PatientManager.Infrastructure.Persistence.Contexts;

namespace PatientManager.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public UserRepository(ApplicationDbContext databaseContext) : base(databaseContext) => _databaseContext = databaseContext;

        public override async Task<User> AddAsync(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            await base.AddAsync(user);
            return user;
        }

        public override async Task UpdateAsync(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            await base.UpdateAsync(user);
        }

        public async Task<User> GetByUsernameAsync(string username) => await _databaseContext.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _databaseContext.Set<User>().FirstOrDefaultAsync(u => u.Username == loginViewModel.Username);

            if (user is not null && PasswordHasher.VerifyPassword(loginViewModel.Password, user.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> UsernameExists(string username) => await _databaseContext.Users.AnyAsync(u => u.Username == username);
    }
}
