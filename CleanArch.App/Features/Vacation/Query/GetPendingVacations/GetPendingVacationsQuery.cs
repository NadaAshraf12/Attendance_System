using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetPendingVacations
{
    public record GetPendingVacationsQuery(Guid ManagerId) : IRequest<ResponseModel>;
}