using Application.DTOs;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using MediatR;

namespace Application.CQRS.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<string>>
    {
        public string Password { get; set; }
    }

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
    {
        private readonly IUsersService _usersService;

        public CreateUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<string>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var createUserDTO = new CreateUserDTO(command.Password);

            var result = await _usersService.CreateAsync(createUserDTO);
            return result;

            //await _unitOfWork.Commit(cancellationToken);
            //return await Result<string>.SuccessAsync();
        }
    }
}