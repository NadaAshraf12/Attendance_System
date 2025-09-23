using CleanArch.App.Services;
using MediatR;
using System;

namespace CleanArch.App.Features.Vacation.Commands.UpdateVacationEndDate
{
    public record UpdateVacationEndDateCommand(
        Guid VacationId,
        DateTime NewEndDate,
        Guid UpdatedByUserId 
    ) : IRequest<ResponseModel>;
}
