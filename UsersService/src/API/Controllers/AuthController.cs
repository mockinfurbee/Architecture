using API.Converters;
using API.DTOs.In;
using Application.Exceptions.User;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using Asp.Versioning;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Logger _logger;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            var message = $"{ex.GetType()}: {ex.Message}";
            if (ex is UserNotFoundException) return NotFound(message);
            else if (ex is InvalidDataException) return BadRequest(message);
            return new ObjectResult(message);
        }

        [HttpPost, Route("Login")]
        public async Task<ActionResult<Result<string>>> Login(AuthUserDTOIn authUserDTOIn, Guid traceId)
        {
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                var result = await _authService.LoginAsync(authUserDTOIn.ToServiceModel());

                var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, authUserDTOIn, result);
                _logger.Info(logInfo);

                if (!result.Succeeded) return Unauthorized(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, authUserDTOIn, ex.Message);
                _logger.Info(logError);
                return GetSuitableAnswerForException(ex);
            }
        }
    }
}
