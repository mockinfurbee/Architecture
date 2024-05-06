using Application.DTOs;
using Application.Interfaces.Services;
using ArchitectureShared;
using MediatR;

namespace Application.CQRS.Users.Commands.DeleteUser
{
    public record DeleteUserCommand : IRequest<Result<string>>
    {
        public DeleteUserCommand(string guid)
        {
            Guid = guid;
        }

        public string Guid { get; set; }
    }

    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly IUsersService _usersService;

        public DeleteUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<string>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var deleteUserDTO = new DeleteUserDTO(command.Guid);

            var result = await _usersService.DeleteAsync(deleteUserDTO.Guid);
            return result;
        }
    }
}
