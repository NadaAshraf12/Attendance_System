using CleanArch.App.Services;
using CleanArch.Common.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Vacation.Command.ReviewVacationRequest
{
    public record ReviewVacationRequestCommand(
    Guid VacationId,
    Guid ApproverId,
    VacationStatus Status,
    string? Reason
) : IRequest<ResponseModel>;

}
