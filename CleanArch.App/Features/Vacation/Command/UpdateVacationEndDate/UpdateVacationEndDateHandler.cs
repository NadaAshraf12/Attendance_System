using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Commands.UpdateVacationEndDate
{
    public class UpdateVacationEndDateHandler
        : IRequestHandler<UpdateVacationEndDateCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public UpdateVacationEndDateHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(UpdateVacationEndDateCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            if (vacation.Status == Common.Enums.VacationStatus.Declined)
                return ResponseModel.Fail("Cannot update a rejected vacation", 400);

            vacation.EndDate = request.NewEndDate;

            vacation.Days = (int)(vacation.EndDate.Date - vacation.StartDate.Date).TotalDays + 1;

            vacation.UpdatedOn = DateTime.UtcNow;
            vacation.UpdatedBy = request.UpdatedByUserId.ToString();

            await _vacationRepository.UpdateAsync(vacation);

            return ResponseModel.Success("Vacation end date updated successfully");
        }
    }
}
