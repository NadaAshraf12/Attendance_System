using CleanArch.App.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.ChangeEmail
{
    public record ChangeEmailCommand(
    string CurrentEmail,  
    string NewEmail       
) : IRequest<ResponseModel>;
}
