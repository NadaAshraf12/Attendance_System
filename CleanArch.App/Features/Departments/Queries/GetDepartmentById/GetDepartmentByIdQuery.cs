using MediatR;

namespace CleanArch.App.Features.Departments.Queries.GetDepartmentById
{
    public record GetDepartmentByIdQuery(Guid Id) : IRequest<CleanArch.App.Services.ResponseModel>;
}
