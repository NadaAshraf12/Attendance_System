using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Users.Commands.AssignRoles
{
    public record AssignRolesCommand(string UserId, IEnumerable<string> Roles) : IRequest<ResponseModel>;
}
