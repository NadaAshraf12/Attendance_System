using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.RefreshToken
{
	public record RefreshTokenCommand(string RefreshToken) : IRequest<ResponseModel>;
}
