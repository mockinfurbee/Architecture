using Application.CQRS.Users.Commands.CreateUser;
using Application.CQRS.Users.Commands.DeleteUser;
using Application.CQRS.Users.Queries.GetAllUsers;
using Application.CQRS.Users.Queries.GetAllUsersWithPagination;
using Application.CQRS.Users.Queries.GetUserByGuid;
using Application.DTOs;
using Application.Exceptions.User;
using ArchitectureSharedLib;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersService.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    // TODO:
    // Swagger custom Descriptions.
    // XMLs for swagger mb.
    // NLog.
    // Improve Dockers mb.
    // Tests: positive and negative.
    // Mb Client-side logic.
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            var message = $"{ex.GetType()}: {ex.Message}";
            if (ex is UserNotFoundException) return NotFound(message);
            else if (ex is InvalidDataException) return BadRequest(message);
            return new ObjectResult(message);
        }

        [HttpGet]
        public async Task<ActionResult<Result<List<GetUserDTO>>>> Get()
        {
            return await _mediator.Send(new GetAllUsersQuery());
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<Result<GetUserDTO>>> GetUserByGuid(string guid)
        {
            try
            { 
                return Ok(await _mediator.Send(new GetUserByGuidQuery(guid)));
            }
            catch (Exception ex)
            {
                return GetSuitableAnswerForException(ex);
            }
        }

        [HttpGet]
        [Route("Paginated")]
        public async Task<ActionResult<PaginatedResult<GetUserDTO>>> GetAllUsersWithPagination([FromQuery] GetUsersWithPaginationQuery query)
        {
            var validator = new GetUsersWithPaginationValidator();

            var result = validator.Validate(query);

            if (result.IsValid) return Ok(await _mediator.Send(query));

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> Create(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{guid}")]
        public async Task<ActionResult<Result<string>>> Delete(string guid)
        {
            try
            {
                return Ok(await _mediator.Send(new DeleteUserCommand(guid)));
            }
            catch (Exception ex)
            {
                return GetSuitableAnswerForException(ex);
            }
        }
    }
}
