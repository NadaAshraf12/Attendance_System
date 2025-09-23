using CleanArch.App.Features.Roles.Commands.CreateRole;
using CleanArch.App.Features.Roles.Commands.DeleteRole;
using CleanArch.App.Features.Users.Commands.AssignRoles;
using CleanArch.App.Features.Users.Commands.ChangePassword;
using CleanArch.App.Features.Users.Commands.SoftDeleteUser;
using CleanArch.App.Features.Users.Commands.UpdateProfile;
using CleanArch.App.Features.Users.Queries.GetUserByEmail;
using CleanArch.App.Features.Users.Queries.GetUserById;
using CleanArch.App.Features.Users.Queries.ListUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(string id)
        => Ok(await _mediator.Send(new GetUserByIdQuery(id)));

    [HttpGet("by-email")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
        => Ok(await _mediator.Send(new GetUserByEmailQuery(email)));

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool includeDeleted = false
    )
        => Ok(await _mediator.Send(new ListUsersQuery(page, pageSize, search, includeDeleted)));

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileCommand cmd, CancellationToken ct)
    => Ok(await _mediator.Send(cmd, ct));


    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd)
        => Ok(await _mediator.Send(cmd));


    [HttpPut("{id}/soft-delete")]
    [Authorize(Roles = "Admin")]
    [ResponseCache]
    //[ProducesResponseType]
    public async Task<IActionResult> SoftDelete(string id, [FromBody] bool isDeleted)
        => Ok(await _mediator.Send(new SoftDeleteUserCommand(id, isDeleted)));

    [HttpPost("add-roles")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoles(string id, [FromBody] IEnumerable<string> roles)
        => Ok(await _mediator.Send(new AssignRolesCommand(id, roles)));



    [Authorize(Roles = "Admin")]
    [HttpDelete("roles/{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
        => Ok(await _mediator.Send(new DeleteRoleCommand(roleName)));


}
