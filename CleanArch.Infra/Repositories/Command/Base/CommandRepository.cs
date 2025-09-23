using CleanArch.Domain.Base;
using CleanArch.Domain.Repositories.Command.Base;
using CleanArch.Infra.Data;

namespace CleanArch.Infra.Repositories.Command.Base
{
    public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity<Guid>
    {
        protected readonly ApplicationDbContext _context;

        public CommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _context.Set<T>().AddAsync(entity, cancellationToken);

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);
    }
}
