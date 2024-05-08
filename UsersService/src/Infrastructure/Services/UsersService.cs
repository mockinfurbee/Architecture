using Application.DTOs;
using Application.Interfaces.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using AutoMapper;

namespace Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO)
        {
            return _unitOfWork.UsersRepository.CreateAsync(createUserDTO);
        }

        public Task<Result<string>> DeleteAsync(string guid)
        {
            return _unitOfWork.UsersRepository.DeleteAsync(guid);
        }

        public async Task<Result<List<GetUserDTO>>> GetAllUsersAsync()
        {
            return await Result<List<GetUserDTO>>.SuccessAsync(_mapper.Map<List<GetUserDTO>>((await _unitOfWork.UsersRepository.GetAllUsersAsync()).Data));
        }

        public async Task<Result<GetUserDTO?>> GetByGuidAsync(string guid)
        {
            return await Result<GetUserDTO?>.SuccessAsync(_mapper.Map<GetUserDTO?>((await _unitOfWork.UsersRepository.GetByGuidAsync(guid)).Data));
        }
    }
}
