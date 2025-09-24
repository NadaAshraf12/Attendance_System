using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Commands.UpdateVacationEndDate
{
    public record UpdateVacationEndDateCommand(
        Guid VacationId,
        DateTime NewEndDate,
        Guid UpdatedByUserId 
    ) : IRequest<ResponseModel>;
}
