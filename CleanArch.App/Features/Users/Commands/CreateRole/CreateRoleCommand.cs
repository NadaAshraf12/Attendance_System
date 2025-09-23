using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Roles.Commands.CreateRole
{
    public record CreateRoleCommand(string RoleName, string Description) : IRequest<ResponseModel>;
}
