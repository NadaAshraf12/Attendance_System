using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Departments.Queries.GetDepartmentByCode
{
    public record GetDepartmentByCodeQuery(string Code) : IRequest<ResponseModel>;

}
