using Application.DTOs;
using Application.Exceptions.User;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            var message = $"{ex.GetType()}: {ex.Message}";
            if (ex is UserNotFoundException) return NotFound(message);
            else if (ex is InvalidDataException) return BadRequest(message);
            return new ObjectResult(message);
        }

        [HttpPost, Route("Login")]
        public async Task<ActionResult<Result<string>>> Login(AuthUserDTO authUserDTO)
        {
            try
            {
                var result = await _authService.LoginAsync(authUserDTO);
                if (!result.Succeeded) return Unauthorized(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return GetSuitableAnswerForException(ex);
            }
        }
    }
}
