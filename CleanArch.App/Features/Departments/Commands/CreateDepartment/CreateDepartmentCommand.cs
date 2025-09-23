using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.CreateDepartment
{
    public record CreateDepartmentCommand(
        string Name,
        string Code,
        string? Description,
        Guid? ParentDepartmentId = null
    ) : IRequest<ResponseModel>;
}
