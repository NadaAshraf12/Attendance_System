using CleanArch.App.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Departments.Commands.Assign_User_to_Department
{
    public record AssignUserToDepartmentCommand(Guid UserId, Guid DepartmentId) : IRequest<ResponseModel>;

}
