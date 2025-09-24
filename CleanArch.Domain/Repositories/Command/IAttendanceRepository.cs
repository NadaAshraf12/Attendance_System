using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories.Command.Base;

namespace CleanArch.Domain.Repositories.Command
{
    public interface IAttendanceRepository : ICommandRepository<AttendanceRecord>
    {
        Task<AttendanceRecord?> GetTodayAttendanceAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<AttendanceRecord>> GetUserAttendanceAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
