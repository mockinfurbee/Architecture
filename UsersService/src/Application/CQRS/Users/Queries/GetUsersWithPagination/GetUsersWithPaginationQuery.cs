using Application.DTOs;
using Application.Extensions;
using Application.Interfaces.Services;
using ArchitectureShared;
using MediatR;

namespace Application.CQRS.Users.Queries.GetAllUsersWithPagination
{
    public record GetUsersWithPaginationQuery : IRequest<PaginatedResult<GetUserDTO>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetUsersWithPaginationQuery() { }

        public GetUsersWithPaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    internal class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, 
                                                                        PaginatedResult<GetUserDTO>>
    {
        private readonly IUsersService _usersService;

        public GetUsersWithPaginationQueryHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<PaginatedResult<GetUserDTO>> Handle(GetUsersWithPaginationQuery query, 
                                                              CancellationToken cancellationToken)
        {
            return (await _usersService.GetAllUsersAsync()).Data.AsQueryable().OrderBy(x => x.Id)
                   .ToPaginatedList(query.PageNumber, query.PageSize);
        }
    }
}
