using Application.DTOs;
using ArchitectureSharedLib;

namespace Application.Interfaces.Services
{
    public interface ISignInService
    {
        public Task<Result<string>> LoginAsync(LoginUserDTO loginUserDTO);

        public Task LogoutAsync();
    }
}