using CleanArch.App.Features.Vacation.Command.ApproveVacation;
using CleanArch.App.Features.Vacation.Command.CreateVacation;
using CleanArch.App.Features.Vacation.Command.DeclineVacation;
using CleanArch.App.Features.Vacation.Command.DeleteVacation;
using CleanArch.App.Features.Vacation.Command.ReviewVacationRequest;
using CleanArch.App.Features.Vacation.Command.UpdateVacation;
using CleanArch.App.Features.Vacation.Commands.UpdateVacationEndDate;
using CleanArch.App.Features.Vacation.Query.GetUserVacations;
using CleanArch.App.Features.Vacation.Query.GetVacationById;
using CleanArch.App.Features.Vacation.Query.GetVacationsByStatus;
using CleanArch.App.Features.Vacation.Query.GetVacationStatistics;
using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VacationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public VacationController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVacation([FromBody] CreateVacationCommand command)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out Guid userId))
            {
                return BadRequest(ResponseModel.Fail("Invalid user ID", 400));
            }

            command = command with { UserId = userId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetVacationById(string id) 
        {
            if (!Guid.TryParse(id, out Guid vacationId))
            {
                return BadRequest(ResponseModel.Fail("Invalid vacation ID", 400));
            }

            var query = new GetVacationByIdQuery(vacationId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetUserVacations(string userId) 
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return BadRequest(ResponseModel.Fail("Invalid user ID", 400));
            }

            var query = new GetUserVacationsQuery(userGuid);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("my-vacations")]
        [Authorize]
        public async Task<IActionResult> GetMyVacations()
        {
            if (!Guid.TryParse(_currentUserService.UserId, out Guid userId))
            {
                return BadRequest(ResponseModel.Fail("Invalid user ID", 400));
            }

            var query = new GetUserVacationsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-status")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetVacationsByStatus([FromQuery] VacationStatus status)
        {
            var result = await _mediator.Send(new GetVacationsByStatusQuery(status));

            if (result.IsError)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("statistics/{year}")]
        [Authorize]
        public async Task<IActionResult> GetVacationStatistics(int year)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out Guid userId))
            {
                return BadRequest(ResponseModel.Fail("Invalid user ID", 400));
            }

            var query = new GetVacationStatisticsQuery(userId, year);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //[HttpPut("approve/{vacationId}")]
        //[Authorize(Roles = "Manager,Admin")]
        //public async Task<IActionResult> ApproveVacation(string vacationId) 
        //{
        //    if (!Guid.TryParse(vacationId, out Guid vacId) || !Guid.TryParse(_currentUserService.UserId, out Guid approverId))
        //    {
        //        return BadRequest(ResponseModel.Fail("Invalid ID", 400));
        //    }

        //    var command = new ApproveVacationCommand(vacId, approverId);
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        //[HttpPut("decline/{vacationId}")]
        //[Authorize(Roles = "Manager,Admin")]
        //public async Task<IActionResult> DeclineVacation(string vacationId, [FromBody] string? reason = null) 
        //{
        //    if (!Guid.TryParse(vacationId, out Guid vacId) || !Guid.TryParse(_currentUserService.UserId, out Guid approverId))
        //    {
        //        return BadRequest(ResponseModel.Fail("Invalid ID", 400));
        //    }

        //    var command = new DeclineVacationCommand(vacId, approverId, reason);
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        [HttpPost("review")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ReviewVacation([FromBody] ReviewVacationRequestCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("{vacationId}")]
        [Authorize]
        public async Task<IActionResult> UpdateVacation(string vacationId, [FromBody] UpdateVacationCommand command) 
        {
            if (!Guid.TryParse(vacationId, out Guid vacId))
            {
                return BadRequest(ResponseModel.Fail("Invalid vacation ID", 400));
            }

            var vacationResult = await _mediator.Send(new GetVacationByIdQuery(vacId));

            command = command with { VacationId = vacId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update-end-date")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateEndDate([FromBody] UpdateVacationEndDateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{vacationId}")]
        [Authorize]
        public async Task<IActionResult> DeleteVacation(string vacationId) 
        {
            if (!Guid.TryParse(vacationId, out Guid vacId))
            {
                return BadRequest(ResponseModel.Fail("Invalid vacation ID", 400));
            }

            var command = new DeleteVacationCommand(vacId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}