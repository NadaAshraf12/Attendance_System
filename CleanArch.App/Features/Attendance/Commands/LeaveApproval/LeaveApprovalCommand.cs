using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.LeaveApproval
{
    public class LeaveApprovalCommand : IRequest<ResponseModel>
    {
        public Guid LeaveRequestId { get; set; } 
        public LeaveStatus Status { get; set; } // Approved or Rejected
    }
}
