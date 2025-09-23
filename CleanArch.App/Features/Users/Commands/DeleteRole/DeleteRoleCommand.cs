using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Roles.Commands.DeleteRole
{
    public record DeleteRoleCommand(string RoleName) : IRequest<ResponseModel>;
}
