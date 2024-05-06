using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Services;
using ArchitectureShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            if (ex is UserNotFoundException) return NotFound(ex.Message);
            else if (ex is InvalidDataException) return BadRequest(ex.Message);
            return new ObjectResult(ex.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> SignIn(LoginUserDTO loginUserDTO)
        {
            try
            {
                return Ok(await _authService.LoginAsync(loginUserDTO));
            }
            catch (Exception ex)
            {
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}
