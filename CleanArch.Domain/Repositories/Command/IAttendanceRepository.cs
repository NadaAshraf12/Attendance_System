using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories.Command.Base;
using CleanArch.Domain.Repositories.Query.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Repositories.Command
{
    public interface IAttendanceRepository : ICommandRepository<AttendanceRecord>
    {
        Task<AttendanceRecord?> GetTodayAttendanceAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<AttendanceRecord>> GetUserAttendanceAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
