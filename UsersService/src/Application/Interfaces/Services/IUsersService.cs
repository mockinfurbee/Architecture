using Application.DTOs;
using ArchitectureShared;

namespace Application.Interfaces.Services
{
    public interface IUsersService
    {
        public Task<Result<List<GetUserDTO?>>> GetAllUsersAsync();
        public Task<Result<GetUserDTO?>> GetByGuidAsync(string guid);

        public Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO);

        public Task<Result<string>> DeleteAsync(string guid);
    }
}
