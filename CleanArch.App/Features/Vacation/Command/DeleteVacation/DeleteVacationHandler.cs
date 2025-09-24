using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.DeleteVacation
{
    public class DeleteVacationHandler : IRequestHandler<DeleteVacationCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public DeleteVacationHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(DeleteVacationCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            if (vacation.Status != Common.Enums.VacationStatus.Pending)
                return ResponseModel.Fail("Can only delete pending vacations", 400);

            await _vacationRepository.DeleteAsync(vacation);
            return ResponseModel.Success(null, "Vacation deleted successfully");
        }
    }
}