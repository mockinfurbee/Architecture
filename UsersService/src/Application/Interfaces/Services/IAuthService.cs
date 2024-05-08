using Application.DTOs;
using ArchitectureSharedLib;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> LoginAsync(AuthUserDTO authUserDTO);
    }
}
