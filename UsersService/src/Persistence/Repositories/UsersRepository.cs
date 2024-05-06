using Application.DTOs;
using Application.Exceptions.User;
using Application.Interfaces.Repositories;
using ArchitectureShared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private async Task<Result<User?>> GetUserByGuidAsync(string guid)
        {
            if (String.IsNullOrWhiteSpace(guid)) throw new InvalidDataException(nameof(guid));

            User? user = await _userManager.FindByIdAsync(guid);

            if (user == null) throw new UserNotFoundException($"{nameof(guid)}: {guid}");

            return await Result<User?>.SuccessAsync(data: user);
        }

        public async Task<Result<GetUserDTO?>> GetByGuidAsync(string guid)
        {
            User? user = (await GetUserByGuidAsync(guid)).Data;

            return await Result<GetUserDTO?>.SuccessAsync(data: _mapper.Map<GetUserDTO>(user));
        }

        public async Task<Result<List<GetUserDTO?>>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            return await Result<List<GetUserDTO?>>.SuccessAsync(data: users.Count() > 0 ? 
                                                                      _mapper.Map<List<GetUserDTO?>>(users) 
                                                                      : new List<GetUserDTO?>());
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
