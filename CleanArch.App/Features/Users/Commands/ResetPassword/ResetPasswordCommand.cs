using CleanArch.App.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(
        string Email,
        string Token,
        string NewPassword
    ) : IRequest<ResponseModel>;
}
