using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ArchitectureShared;

namespace Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO)
        {
            return _unitOfWork.UsersRepository.CreateAsync(createUserDTO);
        }

        public Task<Result<string>> DeleteAsync(string guid)
        {
            return _unitOfWork.UsersRepository.DeleteAsync(guid);
        }

        public Task<Result<List<GetUserDTO?>>> GetAllUsersAsync()
        {
            return _unitOfWork.UsersRepository.GetAllUsersAsync();
        }

        public Task<Result<GetUserDTO?>> GetByGuidAsync(string guid)
        {
            return _unitOfWork.UsersRepository.GetByGuidAsync(guid);
        }
    }
}
