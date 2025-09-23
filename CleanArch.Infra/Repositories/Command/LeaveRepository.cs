using CleanArch.Common.Enums;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LeaveRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaveRequest>> GetUserLeavesAsync(Guid userId)
        {
            return await _dbContext.LeaveRequests
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetPendingLeavesAsync()
        {
            return await _dbContext.LeaveRequests
                .Where(l => l.Status == LeaveStatus.Pending)
                .ToListAsync();
        }

        // ✨ الميثود اللي محتاجها الـ CheckOutHandler
        public async Task<LeaveRequest?> GetApprovedLeaveForUserAsync(
            Guid userId,
            DateTime date,
            CancellationToken cancellationToken)
        {
            return await _dbContext.LeaveRequests
                .FirstOrDefaultAsync(l =>
                    l.UserId == userId &&
                    l.StartDate.Date <= date.Date &&
                    l.EndDate.Date >= date.Date &&
                    l.Status == LeaveStatus.Approved,
                    cancellationToken);
        }

        // ---------- CRUD Implementation ----------
        public async Task AddAsync(LeaveRequest entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.LeaveRequests.AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(LeaveRequest entity, CancellationToken cancellationToken = default)
        {
            _dbContext.LeaveRequests.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(LeaveRequest entity, CancellationToken cancellationToken = default)
        {
            _dbContext.LeaveRequests.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<LeaveRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.LeaveRequests
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

    }
}
