using API.Converters;
using API.DTOs.Out;
using Application.CQRS.Users.Commands.CreateUser;
using Application.CQRS.Users.Commands.DeleteUser;
using Application.CQRS.Users.Queries.GetAllUsers;
using Application.CQRS.Users.Queries.GetAllUsersWithPagination;
using Application.CQRS.Users.Queries.GetUserByGuid;
using Application.Exceptions.User;
using ArchitectureSharedLib;
using Asp.Versioning;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;

namespace UsersService.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            var message = $"{ex.GetType()}: {ex.Message}";
            if (ex is UserNotFoundException) return NotFound(message);
            else if (ex is InvalidDataException) return BadRequest(message);
            return Problem(message);
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<GetUserDTOOut>>>> Get(Guid traceId)
        {
            var sw = new Stopwatch();
            var request = "Get";
            try
            {
                sw.Start();

                var serviceAnswer = await _mediator.Send(new GetAllUsersQuery());
                var result = new Result<List<GetUserDTOOut>>();
                result.Succeeded = true;
                result.Data = serviceAnswer.Data.Select(x => x.ToOutModel()).ToList();

                var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, result);
                _logger.Info(logInfo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, ex.Message);
                _logger.Error(ex);
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<Result<GetUserDTOOut>>> GetUserByGuid(string guid, Guid traceId)
        {
            var sw = new Stopwatch();
            var request = $"GetUserByGuid {guid}";
            try
            {
                sw.Start();

                var serviceAnswer = await _mediator.Send(new GetUserByGuidQuery(guid));
                var result = new Result<GetUserDTOOut>();
                result.Succeeded = true;
                result.Data = new GetUserDTOOut(serviceAnswer.Data.Id);

                var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, result);
                _logger.Info(logInfo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, ex.Message);
                _logger.Error(ex);
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpGet]
        [Route("Paginated")]
        public async Task<ActionResult<PaginatedResult<GetUserDTOOut>>> GetAllUsersWithPagination([FromQuery] GetUsersWithPaginationQuery query, Guid traceId)
        {
            var sw = new Stopwatch();
            try
            {
                sw.Start();

                var validator = new GetUsersWithPaginationValidator();

                var validationResult = validator.Validate(query);

                if (validationResult.IsValid)
                {
                    var serviceAnswer = await _mediator.Send(query);
                    var data = serviceAnswer.Data.Select(x => new GetUserDTOOut(x.Id)).ToList();
                    var result = new PaginatedResult<GetUserDTOOut>(data);
                    result.Succeeded = true;

                    var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, query, result);
                    _logger.Info(logInfo);

                    return Ok(result);
                }

                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                var logE = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, query, errorMessages);
                _logger.Error(logE);

                return BadRequest(errorMessages);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, query, ex.Message);
                _logger.Error(logError);
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> Create(CreateUserCommand command, Guid traceId)
        {
            var sw = new Stopwatch();
            try
            {
                sw.Start();
                var answer = await _mediator.Send(command);

                var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, command, answer);
                _logger.Info(logInfo);

                return Ok(answer);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, command, ex.Message);
                _logger.Error(logError);
                return GetSuitableAnswerForException(ex);
            }
        }

        /// <summary>
        /// You can use this endpoint only if u have logged in via Login endpoint of AuthController.
        /// </summary>
        /// <param name="guid"></param>
        /// <remarks>
        /// Be careful.
        /// </remarks>
        /// <returns><see cref="string"/></returns>
        /// <response code="401">You have to auth via Login endpoint of AuthController.</response>
        /// <response code="404">Pay more attention. UserNotFound:)</response>
        /// <response code="400">You provided invalid guid-value by some way... Most likely it was empty or null.</response> 
        /// <response code="500">Most likely, error on API server-side.</response>  
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{guid}")]
        public async Task<ActionResult<Result<string>>> Delete(string guid, Guid traceId)
        {
            Stopwatch sw = new Stopwatch();
            var request = $"Delete {guid}";
            try
            {
                sw.Start();

                var answer = await _mediator.Send(new DeleteUserCommand(guid));

                var logInfo = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, answer);
                _logger.Info(logInfo);

                return Ok(answer);
            }
            catch (Exception ex)
            {
                var logError = MyLogHelper.StopSwAndGetLogString(traceId.ToString(), sw, request, ex.Message);
                _logger.Info(logError);
                return GetSuitableAnswerForException(ex);
            }
        }
    }
}
