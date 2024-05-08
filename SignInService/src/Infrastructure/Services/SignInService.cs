using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using Microsoft.AspNetCore.Identity;
using Persistence.Entities;

namespace Infrastructure.Services
{
    internal class SignInService : ISignInService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SignInService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<string>> LoginAsync(LoginUserDTO loginUserDTO)
        {
            if (String.IsNullOrWhiteSpace(loginUserDTO.Guid)) throw new InvalidDataException(nameof(loginUserDTO.Guid));

            var user = await _userManager.FindByIdAsync(loginUserDTO.Guid);

            if (user == null) throw new UserNotFoundException($"{nameof(loginUserDTO.Guid)}: {loginUserDTO.Guid}");

            var identityResult = await _signInManager.PasswordSignInAsync(user, loginUserDTO.Password,
                                                                          loginUserDTO.RememberMe, false);
            return identityResult.Succeeded ? await Result<string>.SuccessAsync() : await Result<string>.FailureAsync();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
