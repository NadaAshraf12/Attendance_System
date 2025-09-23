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
    public interface ILeaveRepository : ICommandRepository<LeaveRequest>
    {
        Task<LeaveRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<LeaveRequest?> GetApprovedLeaveForUserAsync(Guid userId, DateTime date, CancellationToken cancellationToken);
        Task<List<LeaveRequest>> GetUserLeavesAsync(Guid userId);
        Task<List<LeaveRequest>> GetPendingLeavesAsync();
    
    }
}
