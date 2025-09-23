using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.DeclineVacation
{
    public class DeclineVacationHandler : IRequestHandler<DeclineVacationCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public DeclineVacationHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(DeclineVacationCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            if (vacation.Status != Common.Enums.VacationStatus.Pending)
                return ResponseModel.Fail("Vacation is not pending approval", 400);

            await _vacationRepository.DeclineAsync(vacation, request.ApproverId, request.Reason);
            return ResponseModel.Success("Vacation declined successfully", null);
        }
    }
}