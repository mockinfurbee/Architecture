using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using AutoMapper;
using NLog;

namespace Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Result<string>> CreateAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                _logger.Info("CreateAsync");
                return _unitOfWork.UsersRepository.CreateAsync(createUserDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public Task<Result<string>> DeleteAsync(string guid)
        {
            try
            {
                _logger.Info("DeleteAsync");
                return _unitOfWork.UsersRepository.DeleteAsync(guid);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public async Task<Result<List<GetUserDTO>>> GetAllUsersAsync()
        {
            try
            {
                _logger.Info("GetAllUsersAsync");
                return await Result<List<GetUserDTO>>.SuccessAsync(_mapper.Map<List<GetUserDTO>>((await _unitOfWork.UsersRepository.GetAllUsersAsync()).Data));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public async Task<Result<GetUserDTO?>> GetByGuidAsync(string guid)
        {
            try
            {
                _logger.Info($"GetByGuidAsync: {guid}");
                return await Result<GetUserDTO?>.SuccessAsync(_mapper.Map<GetUserDTO?>((await _unitOfWork.UsersRepository.GetByGuidAsync(guid)).Data));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
         }
    }
}
