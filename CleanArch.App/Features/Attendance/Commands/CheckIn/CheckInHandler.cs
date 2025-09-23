using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using CleanArch.Domain.Repositories.Command;
using MediatR;

namespace CleanArch.App.Features.Attendance.Commands.CheckIn
{
    public class CheckInHandler : IRequestHandler<CheckInCommand, ResponseModel>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IVacationRepository _vacationRepository;
        private readonly ICurrentUserService _currentUserService;

        public CheckInHandler(
            IAttendanceRepository attendanceRepository,
            IVacationRepository vacationRepository,
            ICurrentUserService currentUserService)
        {
            _attendanceRepository = attendanceRepository;
            _vacationRepository = vacationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseModel> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            // Validate userId from token
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                return ResponseModel.Fail("Invalid user ID in token");
            }

            // 1. Check if user has approved vacation today
            var hasVacation = await _vacationRepository.HasVacationOnDateAsync(
                userId,
                request.CheckInTime.Date,
                cancellationToken);

            if (hasVacation)
            {
                return ResponseModel.Fail("You are on vacation, check-in is not allowed");
            }

            // 2. Check if already checked in today
            var existingRecord = await _attendanceRepository.GetTodayAttendanceAsync(userId, cancellationToken);
            if (existingRecord != null)
            {
                return ResponseModel.Fail("Already checked in today");
            }

            // 3. Create new attendance record
            var attendance = new AttendanceRecord
            {
                UserId = userId,
                CheckInTime = request.CheckInTime,
                Status = DetermineAttendanceStatus(request.CheckInTime)
            };

            await _attendanceRepository.AddAsync(attendance, cancellationToken);
            await _attendanceRepository.SaveChangesAsync(cancellationToken);

            return ResponseModel.Success("Checked in successfully");
        }

        private AttendanceStatus DetermineAttendanceStatus(DateTime checkInTime)
        {
            var startTime = new DateTime(checkInTime.Year, checkInTime.Month, checkInTime.Day, 9, 0, 0);
            var latestOnTime = new DateTime(checkInTime.Year, checkInTime.Month, checkInTime.Day, 10, 0, 0);

            if (checkInTime <= latestOnTime)
                return AttendanceStatus.Present;

            return AttendanceStatus.Late;
        }
    }
}
