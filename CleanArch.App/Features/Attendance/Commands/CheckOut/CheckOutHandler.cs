using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Infra.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace CleanArch.App.Features.Attendance.Commands.CheckOut
{
    public class CheckOutHandler : IRequestHandler<CheckOutCommand, ResponseModel>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocationService _locationService;
        private readonly CompanyLocationOptions _companyLocation;

        public CheckOutHandler(
            IAttendanceRepository attendanceRepository,
            ILeaveRepository leaveRepository,
            ICurrentUserService currentUserService,
            ILocationService locationService,
            IOptions<CompanyLocationOptions> companyLocationOptions)
        {
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
            _currentUserService = currentUserService;
            _locationService = locationService;
            _companyLocation = companyLocationOptions.Value;
        }

        public async Task<ResponseModel> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _currentUserService.UserId;

            if (!Guid.TryParse(userIdString, out var userId))
                return ResponseModel.Fail("Invalid user id");

            // Validate location if provided
            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                var userLocation = new LocationDto
                {
                    Latitude = request.Latitude.Value,
                    Longitude = request.Longitude.Value
                };

                var companyLocation = new LocationDto
                {
                    Latitude = _companyLocation.Latitude,
                    Longitude = _companyLocation.Longitude
                };

                if (!_locationService.IsWithinRadius(userLocation, companyLocation, _companyLocation.AllowedRadiusInMeters))
                {
                    var distance = _locationService.CalculateDistance(userLocation, companyLocation);
                    return ResponseModel.Fail($"You are too far from company location to check out. Distance: {distance:F0} meters");
                }
            }

            var attendance = await _attendanceRepository.GetTodayAttendanceAsync(userId, cancellationToken);

            if (attendance == null)
                return ResponseModel.Fail("No check-in found for today");

            if (attendance.CheckOutTime != null)
                return ResponseModel.Fail("Already checked out today");

            // Set checkout time and location data
            attendance.CheckOutTime = request.CheckOutTime;
            attendance.CheckOutLatitude = request.Latitude;
            attendance.CheckOutLongitude = request.Longitude;
            attendance.CheckOutDeviceInfo = request.DeviceInfo;
            attendance.CheckOutIpAddress = request.IpAddress;

            var workedHours = (attendance.CheckOutTime.Value - attendance.CheckInTime).TotalHours;

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
                attendance.Status = AttendanceStatus.Present;
            }

            await _attendanceRepository.UpdateAsync(attendance, cancellationToken);
            await _attendanceRepository.SaveChangesAsync(cancellationToken);

            return ResponseModel.Success("Checked out successfully");
        }
    }
}