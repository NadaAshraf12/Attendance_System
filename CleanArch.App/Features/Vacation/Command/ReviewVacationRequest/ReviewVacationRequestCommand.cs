using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.ReviewVacationRequest
{
    public record ReviewVacationRequestCommand(
    Guid VacationId,
    Guid ApproverId,
    VacationStatus Status,
    string? Reason
) : IRequest<ResponseModel>;

}
