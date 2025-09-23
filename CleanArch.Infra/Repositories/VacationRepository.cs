using CleanArch.Common.Enums;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using CleanArch.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly ApplicationDbContext _context;
        public VacationRepository(ApplicationDbContext context) => _context = context;

        public async Task<Vacation> AddAsync(Vacation vacation)
        {
            vacation.Days = (int)(vacation.EndDate.Date - vacation.StartDate.Date).TotalDays + 1;
            vacation.CreatedOn = DateTime.UtcNow;
            _context.Vacations.Add(vacation);
            await _context.SaveChangesAsync();
            return vacation;
        }

        public Task<Vacation?> GetByIdAsync(Guid id) =>
            _context.Vacations.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

        public Task<Vacation?> GetByIdWithUserAsync(Guid id) =>
            _context.Vacations.Include(v => v.User).Include(v => v.Substitute)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);

        public async Task<IEnumerable<Vacation>> GetPendingForManagerAsync(Guid managerId)
        {
            var manager = await _context.Users.Include(u => u.Department)
                                              .FirstOrDefaultAsync(u => u.Id == managerId);
            if (manager == null) return Enumerable.Empty<Vacation>();

            var deptId = manager.DepartmentId;
            return await _context.Vacations
                .Include(v => v.User)
                .Where(v => v.Status == VacationStatus.Pending &&
                            v.User.DepartmentId == deptId &&
                            !v.IsDeleted)
                .ToListAsync();
        }

        public async Task ApproveAsync(Vacation vacation, Guid approverId)
        {
            vacation.Status = VacationStatus.Accepted;
            vacation.ApprovedById = approverId;
            vacation.ApprovedAt = DateTime.UtcNow;
            vacation.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeclineAsync(Vacation vacation, Guid approverId, string? reason)
        {
            vacation.Status = VacationStatus.Declined;
            vacation.ApprovedById = approverId;
            vacation.UpdatedOn = DateTime.UtcNow;
            // ممكن نخزن decline reason في عمود لو حابين
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vacation>> GetByUserAsync(Guid userId) =>
            await _context.Vacations.Where(v => v.UserId == userId && !v.IsDeleted).ToListAsync();

        public async Task UpdateAsync(Vacation vacation)
        {
            vacation.UpdatedOn = DateTime.UtcNow;
            _context.Vacations.Update(vacation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vacation vacation)
        {
            vacation.IsDeleted = true;
            vacation.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasVacationOnDateAsync(Guid userId, DateTime date, CancellationToken cancellationToken)
        {
            return await _context.Vacations
                .AnyAsync(v => v.UserId == userId
                            && v.Status == VacationStatus.Accepted
                            && v.StartDate <= date
                            && v.EndDate >= date
                            && !v.IsDeleted,
                          cancellationToken);
        }

    }
}
