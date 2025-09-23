using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.ApproveVacation
{
    public record ApproveVacationCommand(Guid VacationId, Guid ApproverId) : IRequest<ResponseModel>;
}