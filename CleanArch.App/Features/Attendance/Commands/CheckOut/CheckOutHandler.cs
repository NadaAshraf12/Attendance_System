using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories.Command;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.CheckOut
{
    public class CheckOutHandler : IRequestHandler<CheckOutCommand, ResponseModel>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ICurrentUserService _currentUserService;

        public CheckOutHandler(
            IAttendanceRepository attendanceRepository,
            ILeaveRepository leaveRepository,
            ICurrentUserService currentUserService)
        {
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseModel> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            // Get current user Id
            var userIdString = _currentUserService.UserId;

            if (!Guid.TryParse(userIdString, out var userId))
                return ResponseModel.Fail("Invalid user id");

            // Get today's attendance
            var attendance = await _attendanceRepository.GetTodayAttendanceAsync(userId, cancellationToken);

            if (attendance == null)
                return ResponseModel.Fail("No check-in found for today");

            if (attendance.CheckOutTime != null)
                return ResponseModel.Fail("Already checked out today");

            // Set checkout time
            attendance.CheckOutTime = request.CheckOutTime;

            // احسب عدد الساعات اللي اشتغلها
            var workedHours = (attendance.CheckOutTime.Value - attendance.CheckInTime).TotalHours;

            // لو أقل من 8 ساعات → لازم يكون عامل LeaveRequest وموافق عليه
            if (workedHours < 8)
            {
                var leaveRequest = await _leaveRepository.GetApprovedLeaveForUserAsync(
                    userId,
                    attendance.CheckInTime.Date,
                    cancellationToken);

                if (leaveRequest == null)
                {
                    return ResponseModel.Fail("You cannot check out early without an approved leave request");
                }

                attendance.Status = AttendanceStatus.OnLeave;
            }
            else
            {
                attendance.Status = AttendanceStatus.Present; // اليوم خلص طبيعي
            }

            // Save changes
            await _attendanceRepository.UpdateAsync(attendance, cancellationToken);
            await _attendanceRepository.SaveChangesAsync(cancellationToken);

            return ResponseModel.Success("Checked out successfully");
        }
    }
}
