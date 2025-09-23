using CleanArch.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
