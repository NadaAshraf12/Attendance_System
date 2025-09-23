using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetUserVacations
{
    public record GetUserVacationsQuery(Guid UserId) : IRequest<ResponseModel>;
}