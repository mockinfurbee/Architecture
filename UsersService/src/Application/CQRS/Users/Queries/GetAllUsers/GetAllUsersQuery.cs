using Application.DTOs;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using MediatR;

namespace Application.CQRS.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<Result<List<GetUserDTO>>>;

    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<GetUserDTO>>>
    {
        private readonly IUsersService _usersService;

        public GetAllUsersQueryHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<List<GetUserDTO>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            //var players = await _unitOfWork.Repository<Player>().Entities
            //.ProjectTo<GetAllPlayersDto>(_mapper.ConfigurationProvider)
            //.ToListAsync(cancellationToken);

            return await _usersService.GetAllUsersAsync();
        }
    }
}
