using CleanArch.Domain.Base;

namespace CleanArch.Domain.Repositories.Command.Base
{
    public interface ICommandRepository<T> where T : BaseEntity<Guid>
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
