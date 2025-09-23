using CleanArch.App.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Departments.Queries.GetDepartmentByCode
{
    public record GetDepartmentByCodeQuery(string Code) : IRequest<ResponseModel>;

}
