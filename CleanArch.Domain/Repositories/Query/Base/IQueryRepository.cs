using CleanArch.Domain.Base;
using CleanArch.Domain.Specifications;

namespace CleanArch.Domain.Repositories.Query.Base
{
    public interface IQueryRepository<T> where T : BaseEntity<Guid>
    {
        IQueryable<T> Table { get; }
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    }
}
