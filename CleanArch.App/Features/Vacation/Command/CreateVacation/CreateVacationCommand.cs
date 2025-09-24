using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.CreateVacation
{
    public record CreateVacationCommand(
        Guid UserId,
        VacationType Type,
        DateTime StartDate,
        DateTime EndDate,
        Guid? SubstituteId,
        string? Reason,
        string? AttachmentPath
    ) : IRequest<ResponseModel>;
}
