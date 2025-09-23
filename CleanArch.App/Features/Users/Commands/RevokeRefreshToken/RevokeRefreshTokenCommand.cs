using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.RevokeRefreshToken
{
    public record RevokeRefreshTokenCommand(string RefreshToken) : IRequest<ResponseModel>;
}
