// CleanArch.Api/Controllers/AttendanceController.cs
using CleanArch.App.Features.Attendance.Commands.CheckIn;
using CleanArch.App.Features.Attendance.Commands.CheckOut;
using CleanArch.App.Features.Attendance.Commands.LeaveApproval;
using CleanArch.App.Features.Attendance.Commands.LeaveRequests;
using CleanArch.App.Features.Attendance.Queries.GetPendingLeaves;
using CleanArch.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // لازم يكون المستخدم لوج إن ومعاه JWT
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(IMediator mediator, ILogger<AttendanceController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn(
            [FromBody] CheckInRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Check-in request received from user: {UserId}",
                    User.FindFirst("sub")?.Value);

                var command = new CheckInCommand
                {
                    CheckInTime = request.CheckInTime ?? DateTime.Now,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    DeviceInfo = request.DeviceInfo ?? GetDeviceInfo(),
                    IpAddress = request.IpAddress ?? GetClientIpAddress()
                };

                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsError)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during check-in for user: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred during check-in");
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut(
            [FromBody] CheckOutRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Check-out request received from user: {UserId}",
                    User.FindFirst("sub")?.Value);

                var command = new CheckOutCommand
                {
                    CheckOutTime = request.CheckOutTime ?? DateTime.Now,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    DeviceInfo = request.DeviceInfo ?? GetDeviceInfo(),
                    IpAddress = request.IpAddress ?? GetClientIpAddress()
                };

                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsError)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during check-out for user: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred during check-out");
            }
        }

        [HttpPost("leave-request")]
        public async Task<IActionResult> LeaveRequest(
            [FromBody] LeaveRequestCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Leave request submitted by user: {UserId}",
                    User.FindFirst("sub")?.Value);

                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsError)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting leave request for user: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred while submitting leave request");
            }
        }

        [HttpPost("leave-approval")]
        [Authorize(Roles = "Admin,Manager")] // الادمن والمانجر يقدر يوافق
        public async Task<IActionResult> LeaveApproval(
            [FromBody] LeaveApprovalCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Leave approval action by manager: {UserId}",
                    User.FindFirst("sub")?.Value);

                var result = await _mediator.Send(command, cancellationToken);

                if (result.IsError)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during leave approval by manager: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred during leave approval");
            }
        }

        [HttpGet("pending-leaves")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetPendingLeaves(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Pending leaves query by manager: {UserId}",
                    User.FindFirst("sub")?.Value);

                var leaves = await _mediator.Send(new GetPendingLeavesQuery(), cancellationToken);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending leaves for manager: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred while fetching pending leaves");
            }
        }

        [HttpGet("today-attendance")]
        public async Task<IActionResult> GetTodayAttendance(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Today's attendance query by user: {UserId}",
                    User.FindFirst("sub")?.Value);

                // هتحتاج تعمل Query جديد لهذا الغرض
                // var attendance = await _mediator.Send(new GetTodayAttendanceQuery(), cancellationToken);
                // return Ok(attendance);

                return Ok(new { message = "Feature coming soon" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching today's attendance for user: {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An error occurred while fetching attendance");
            }
        }

        // Helper methods
        private string GetDeviceInfo()
        {
            return $"{Request.Headers["User-Agent"]}";
        }

        private string GetClientIpAddress()
        {
            // Try to get IP from headers (for proxy scenarios)
            var ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
                ip = Request.Headers["X-Real-IP"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            return ip ?? "Unknown";
        }
    }
}