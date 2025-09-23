using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.Logout
{
    public record LogoutCommand(string RefreshToken, bool AllDevices = false) : IRequest<ResponseModel>;
}
