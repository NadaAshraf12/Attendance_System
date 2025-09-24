using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories.Command;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.LeaveApproval
{
    public class LeaveApprovalHandler : IRequestHandler<LeaveApprovalCommand, ResponseModel>
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveApprovalHandler(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<ResponseModel> Handle(LeaveApprovalCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRepository.GetByIdAsync(request.LeaveRequestId, cancellationToken);

            if (leaveRequest == null)
                return ResponseModel.Fail("Leave request not found");

            if (leaveRequest.Status != LeaveStatus.Pending)
                return ResponseModel.Fail("Leave request already processed");

            leaveRequest.Status = request.Status;

            await _leaveRepository.UpdateAsync(leaveRequest, cancellationToken);
            await _leaveRepository.SaveChangesAsync(cancellationToken);

            return ResponseModel.Success($"Leave request {request.Status} successfully");
        }
    }
}
