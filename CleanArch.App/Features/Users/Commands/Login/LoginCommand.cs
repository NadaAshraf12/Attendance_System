using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password,
        string? Device,
        string? DeviceId,
        string? IpAddress
    ) : IRequest<ResponseModel>;
}
