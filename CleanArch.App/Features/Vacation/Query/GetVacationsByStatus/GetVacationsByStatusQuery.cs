using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationsByStatus
{
    public record GetVacationsByStatusQuery(VacationStatus Status) : IRequest<ResponseModel>;
}
