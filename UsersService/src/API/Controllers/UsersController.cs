using Application.CQRS.Users.Commands.CreateUser;
using Application.CQRS.Users.Commands.DeleteUser;
using Application.CQRS.Users.Queries.GetAllUsers;
using Application.CQRS.Users.Queries.GetAllUsersWithPagination;
using Application.CQRS.Users.Queries.GetUserByGuid;
using Application.DTOs;
using Application.Exceptions.User;
using ArchitectureShared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UsersService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // TODO:
    // Improve Dockers.
    // Swagger versionin + Authorize button, Descriptions.
    // NLog/SeriLog.
    // Tests: positive and negative.
    // Mb Client-side.
    // XMLs mb.
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private ActionResult GetSuitableAnswerForException(Exception ex)
        {
            if (ex is UserNotFoundException) return NotFound(ex.Message);
            else if (ex is InvalidDataException) return BadRequest(ex.Message);
            return new ObjectResult(ex.Message);
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
