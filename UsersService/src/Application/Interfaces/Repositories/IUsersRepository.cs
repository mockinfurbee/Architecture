using Application.DTOs;
using Application.Interfaces.Entities;
using ArchitectureSharedLib;

namespace Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task<Result<List<IUser>>> GetAllUsersAsync();
        public Task<Result<IUser?>> GetByGuidAsync(string guid);

        public Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO);

        public Task<Result<string>> DeleteAsync(string guid);
    }
}
