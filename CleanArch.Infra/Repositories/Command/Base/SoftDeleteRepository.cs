using CleanArch.Domain.Base;
using CleanArch.Domain.Repositories.Command.Base;
using CleanArch.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Repositories.Command.Base
{
    public class SoftDeleteRepository<T> : ISoftDeleteRepository<T> where T : BaseEntity<Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public SoftDeleteRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedOn = DateTime.UtcNow;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
