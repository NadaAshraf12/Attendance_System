using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Departments.Commands.Assign_User_to_Department
{
    public record AssignUserToDepartmentCommand(Guid UserId, Guid DepartmentId) : IRequest<ResponseModel>;

}
