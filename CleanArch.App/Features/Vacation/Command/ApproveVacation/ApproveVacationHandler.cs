using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.ApproveVacation
{
    public class ApproveVacationHandler : IRequestHandler<ApproveVacationCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public ApproveVacationHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(ApproveVacationCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            if (vacation.Status != Common.Enums.VacationStatus.Pending)
                return ResponseModel.Fail("Vacation is not pending approval", 400);

            await _vacationRepository.ApproveAsync(vacation, request.ApproverId);
            return ResponseModel.Success("Vacation approved successfully" , null);
        }
    }
}