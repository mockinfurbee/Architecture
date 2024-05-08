using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ArchitectureSharedLib;
using MediatR;

namespace Application.CQRS.Users.Queries.GetUserByGuid
{
    public record GetUserByGuidQuery : IRequest<Result<GetUserDTO>>
    {
        public string Guid { get; set; }

        public GetUserByGuidQuery(string guid)
        {
            Guid = guid;
        }
    }

    internal class GetUserByGuidQueryHandler : IRequestHandler<GetUserByGuidQuery, Result<GetUserDTO>>
    {
        private readonly IUsersService _usersService;

        public GetUserByGuidQueryHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<GetUserDTO>> Handle(GetUserByGuidQuery query, CancellationToken cancellationToken)
        {
            //    var entity = await _unitOfWork.Repository<Player>().GetByGuidAsync(query.Guid);
            //    var player = _mapper.Map<GetPlayerByIdDto>(entity);
            return await _usersService.GetByGuidAsync(query.Guid);
        }
    }
}
