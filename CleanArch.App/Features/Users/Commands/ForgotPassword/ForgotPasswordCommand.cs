using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<ResponseModel>;
}
