using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.DeclineVacation
{
    public record DeclineVacationCommand(Guid VacationId, Guid ApproverId, string? Reason) : IRequest<ResponseModel>;
}