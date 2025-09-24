using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.ChangeEmail
{
    public record ChangeEmailCommand(
    string CurrentEmail,  
    string NewEmail       
) : IRequest<ResponseModel>;
}
