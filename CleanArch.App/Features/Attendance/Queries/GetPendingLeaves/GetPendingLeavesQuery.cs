using CleanArch.Domain.Entities;
using MediatR;

namespace CleanArch.App.Features.Attendance.Queries.GetPendingLeaves
{
    public class GetPendingLeavesQuery : IRequest<List<LeaveRequest>>
    {
    }
}
