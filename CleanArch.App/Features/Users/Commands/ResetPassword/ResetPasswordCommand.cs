using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(
        string Email,
        string Token,
        string NewPassword
    ) : IRequest<ResponseModel>;
}
