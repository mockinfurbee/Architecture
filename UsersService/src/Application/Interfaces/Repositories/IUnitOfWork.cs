using ArchitectureCommonDomainLib.Common;

namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IUsersRepository UsersRepository { get; }
        public IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;
        public Task<int> Commit(CancellationToken cancellationToken);
        //public Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        public Task Rollback();
    }
}
