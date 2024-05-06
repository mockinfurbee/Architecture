using Application.DTOs;
using Application.Interfaces.Repositories;
using ArchitectureShared;
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
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByGuidQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetUserDTO>> Handle(GetUserByGuidQuery query, CancellationToken cancellationToken)
        {
            //    var entity = await _unitOfWork.Repository<Player>().GetByIdAsync(query.Guid);
            //    var player = _mapper.Map<GetPlayerByIdDto>(entity);
            return await _unitOfWork.UsersRepository.GetByGuidAsync(query.Guid);
        }
    }
}
