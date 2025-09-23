using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationById
{
    public record GetVacationByIdQuery(Guid VacationId) : IRequest<ResponseModel>;
}