using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmNewPassword) : IRequest<ResponseModel>;
}
