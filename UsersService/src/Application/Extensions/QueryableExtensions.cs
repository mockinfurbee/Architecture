using ArchitectureShared;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginatedResult<T> ToPaginatedList<T>(this IQueryable<T> source, int pageNumber, 
                                                                             int pageSize) where T : class
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = source.Count();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return PaginatedResult<T>.Create(items, count, pageNumber, pageSize);
        }
    }
}
