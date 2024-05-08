using Application.DTOs;
using Application.Exceptions.User;
using ArchitectureSharedLib;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Repositories;
using Infrastructure.Entities;
using Application.Interfaces.Entities;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;

        public UsersRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<IUser?>> GetByGuidAsync(string guid)
        {
            if (String.IsNullOrWhiteSpace(guid)) throw new InvalidDataException(nameof(guid));

            User? user = await _userManager.FindByIdAsync(guid);

            if (user == null) throw new UserNotFoundException($"{nameof(guid)}: {guid}");

            return await Result<IUser?>.SuccessAsync(data: user);
        }

        public async Task<Result<List<IUser>>> GetAllUsersAsync()
        {
            var users = (_userManager.Users as IQueryable<IUser>).ToList();
            return await Result<List<IUser>>.SuccessAsync(data: users);
        }

        public async Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO)
        {
            User user = new User();

            var identityResult = await _userManager.CreateAsync(user, createUserDTO.Password);
            if (identityResult.Succeeded) return await Result<string>.SuccessAsync(data: String.Empty);

            return await Result<string>.FailureAsync(data: String.Empty);
        }

        public async Task<Result<string>> DeleteAsync(string guid)
        {
            if (String.IsNullOrWhiteSpace(guid)) throw new InvalidDataException(nameof(guid));

            User? user = await _userManager.FindByIdAsync(guid);

            if (user == null) throw new UserNotFoundException($"{nameof(guid)}: {guid}");

            var identityResult = await _userManager.DeleteAsync(user);
            if (identityResult.Succeeded) return await Result<string>.SuccessAsync(data: String.Empty);

            return await Result<string>.FailureAsync(data: String.Empty);
        }
    }
}
