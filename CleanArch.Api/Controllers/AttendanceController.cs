using CleanArch.App.Features.Attendance.Commands.CheckIn;
using CleanArch.App.Features.Attendance.Commands.CheckOut;
using CleanArch.App.Features.Attendance.Commands.LeaveApproval;
using CleanArch.App.Features.Attendance.Commands.LeaveRequests;
using CleanArch.App.Features.Attendance.Queries.GetPendingLeaves;
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

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn(CancellationToken cancellationToken)
        {
            // مفيش UserId في الـ request لان احنا بناخده من الـ Token
            var command = new CheckInCommand();

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut(CancellationToken cancellationToken)
        {
            var command = new CheckOutCommand();
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("leave-request")]
        public async Task<IActionResult> LeaveRequest([FromBody] LeaveRequestCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("leave-approval")]
        [Authorize(Roles = "Admin")] // بس الادمن يقدر
        public async Task<IActionResult> LeaveApproval([FromBody] LeaveApprovalCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsError)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("pending-leaves")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingLeaves(CancellationToken cancellationToken)
        {
            var leaves = await _mediator.Send(new GetPendingLeavesQuery(), cancellationToken);
            return Ok(leaves);
        }



    }
}
