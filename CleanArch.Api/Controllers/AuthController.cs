using CleanArch.App.Features.Users.Commands.ChangeEmail;
using CleanArch.App.Features.Users.Commands.ForgotPassword;
using CleanArch.App.Features.Users.Commands.Login;
using CleanArch.App.Features.Users.Commands.Logout;
using CleanArch.App.Features.Users.Commands.RefreshToken;
using CleanArch.App.Features.Users.Commands.RegisterUser;
using CleanArch.App.Features.Users.Commands.ResetPassword;
using CleanArch.App.Features.Users.Commands.RevokeRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeRefreshTokenCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [Authorize]
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand cmd)
    => Ok(await _mediator.Send(cmd));

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsError)
            return BadRequest(result);

        return Ok(result);
    }



}
