using Application.DTOs;
using ArchitectureShared;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> LoginAsync(LoginUserDTO loginUserDTO);

        public Task LogoutAsync();
    }
}