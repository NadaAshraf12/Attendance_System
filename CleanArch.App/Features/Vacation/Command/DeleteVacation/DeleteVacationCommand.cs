using CleanArch.App.Services;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.DeleteVacation
{
    public record DeleteVacationCommand(Guid VacationId) : IRequest<ResponseModel>;
}