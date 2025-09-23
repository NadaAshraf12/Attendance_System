using CleanArch.Domain.Base;
using CleanArch.Domain.Repositories.Query.Base;
using CleanArch.Domain.Specifications;
using CleanArch.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Repositories.Query.Base
{
    public class QueryRepository<T> : IQueryRepository<T> where T : BaseEntity<Guid>
    {
        protected readonly ApplicationDbContext _context;

        public QueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Table => _context.Set<T>().AsNoTracking();

        public async Task<T> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync()
            => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).ToListAsync();

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
