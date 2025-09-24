using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Common.Enums;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using CleanArch.Domain.Repositories.Command;
using CleanArch.Infra.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace CleanArch.App.Features.Attendance.Commands.CheckIn
{
    public class CheckInHandler : IRequestHandler<CheckInCommand, ResponseModel>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IVacationRepository _vacationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocationService _locationService;
        private readonly CompanyLocationOptions _companyLocation;

        public CheckInHandler(
            IAttendanceRepository attendanceRepository,
            IVacationRepository vacationRepository,
            ICurrentUserService currentUserService,
            ILocationService locationService,
            IOptions<CompanyLocationOptions> companyLocationOptions)
        {
            _attendanceRepository = attendanceRepository;
            _vacationRepository = vacationRepository;
            _currentUserService = currentUserService;
            _locationService = locationService;
            _companyLocation = companyLocationOptions.Value;
        }

        public async Task<ResponseModel> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                return ResponseModel.Fail("Invalid user ID in token");
            }

            // 1. Validate location if provided
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
                    return ResponseModel.Fail($"You are too far from company location. Distance: {distance:F0} meters. Allowed radius: {_companyLocation.AllowedRadiusInMeters} meters");
                }
            }

            // 2. Check if user has approved vacation today
            var hasVacation = await _vacationRepository.HasVacationOnDateAsync(
                userId,
                request.CheckInTime.Date,
                cancellationToken);

            if (hasVacation)
            {
                return ResponseModel.Fail("You are on vacation, check-in is not allowed");
            }

            // 3. Check if already checked in today
            var existingRecord = await _attendanceRepository.GetTodayAttendanceAsync(userId, cancellationToken);
            if (existingRecord != null)
            {
                return ResponseModel.Fail("Already checked in today");
            }

            // 4. Create new attendance record with location data
            var attendance = new AttendanceRecord
            {
                UserId = userId,
                CheckInTime = request.CheckInTime,
                CheckInLatitude = request.Latitude,
                CheckInLongitude = request.Longitude,
                CheckInDeviceInfo = request.DeviceInfo,
                CheckInIpAddress = request.IpAddress,
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