using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Infra.Data;
using CleanArch.Infra.Repositories.Command.Base;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Repositories.Command
{
    public class AttendanceRepository
    : CommandRepository<AttendanceRecord>,
      IAttendanceRepository
    {
        public AttendanceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<AttendanceRecord?> GetTodayAttendanceAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var today = DateTime.Today;
            return await _context.AttendanceRecords
                .FirstOrDefaultAsync(a => a.UserId == userId &&
                                          a.CheckInTime.Date == today,
                                          cancellationToken);
        }

        public async Task<List<AttendanceRecord>> GetUserAttendanceAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.AttendanceRecords
                .Where(a => a.UserId == userId &&
                            a.CheckInTime.Date >= startDate.Date &&
                            a.CheckInTime.Date <= endDate.Date)
                .ToListAsync(cancellationToken);
        }
    }

}
