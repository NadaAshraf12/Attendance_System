using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.DeleteDepartment
{
    public record DeleteDepartmentCommand(Guid Id) : IRequest<ResponseModel>;
}
