using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Domain.Entities;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.LeaveRequests
{
    public class LeaveRequestHandler : IRequestHandler<LeaveRequestCommand, ResponseModel>
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ICurrentUserService _currentUserService;

        public LeaveRequestHandler(
            ILeaveRepository leaveRepository,
            ICurrentUserService currentUserService)
        {
            _leaveRepository = leaveRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseModel> Handle(LeaveRequestCommand request, CancellationToken cancellationToken)
        {
            // Get current user Id
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
                return ResponseModel.Fail("Invalid user id");

            // Validation: StartDate must be before EndDate
            if (request.StartDate > request.EndDate)
                return ResponseModel.Fail("Start date cannot be after end date");

            var leaveRequest = new LeaveRequest
            {
                UserId = userId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                LeaveType = request.LeaveType,
                Reason = request.Reason,
                Status = LeaveStatus.Pending // default
            };

            await _leaveRepository.AddAsync(leaveRequest, cancellationToken);
            await _leaveRepository.SaveChangesAsync(cancellationToken);

            return ResponseModel.Success("Leave request submitted successfully and is pending approval.");
        }
    }
}
