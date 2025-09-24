using CleanArch.Common.Dtos;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.UpdateDepartment
{
    public record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    bool IsActive,
    List<SubDepartmentDto>? SubDepartments
) : IRequest<CleanArch.App.Services.ResponseModel>;

    

}
