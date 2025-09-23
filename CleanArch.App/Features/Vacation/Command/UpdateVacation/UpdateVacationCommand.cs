using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.UpdateVacation
{
    public record UpdateVacationCommand(
        Guid VacationId,
        VacationType? Type,
        DateTime? StartDate,
        DateTime? EndDate,
        Guid? SubstituteId,
        string? Reason
    ) : IRequest<ResponseModel>;
}