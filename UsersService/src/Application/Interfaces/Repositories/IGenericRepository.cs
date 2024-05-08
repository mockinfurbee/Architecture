using ArchitectureCommonDomainLib.Common.Interfaces;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        public IQueryable<T> Entities { get; }

        public Task<T> GetByGuidAsync(string guid);
        public Task<List<T>> GetAllAsync();
        public Task<T> AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    }
}
