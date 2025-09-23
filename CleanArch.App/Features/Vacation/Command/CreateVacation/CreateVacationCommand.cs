using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
