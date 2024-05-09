using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AuthService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task<Result<string>> LoginAsync(AuthUserDTO authUserDTO)
        {
            try
            {
                #region logInfo
                _logger.Info($"LoginAsync with user guid {authUserDTO.Guid}");
                #endregion

                if (authUserDTO == null) throw new InvalidDataException("Invalid client request.");

                var user = (User)(await _unitOfWork.UsersRepository.GetByGuidAsync(authUserDTO.Guid)).Data;

                var passwordHasher = _serviceProvider.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authUserDTO.Password) == PasswordVerificationResult.Success)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("TokenValidationParams:SecretKey")));
                    var authCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                        issuer: _configuration.GetValue<string>("TokenValidationParams:ValidIssuer"),
                        audience: _configuration.GetValue<string>("TokenValidationParams:ValidAudience"),
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: authCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return await Result<string>.SuccessAsync(tokenString);
                }

                return await Result<string>.FailureAsync("Passwords are not match.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
