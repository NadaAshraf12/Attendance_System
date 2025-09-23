using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationStatistics
{
    public record GetVacationStatisticsQuery(Guid UserId, int Year) : IRequest<ResponseModel>;
}