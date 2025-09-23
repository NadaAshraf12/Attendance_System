using CleanArch.App.Services;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.UpdateVacation
{
    public class UpdateVacationHandler : IRequestHandler<UpdateVacationCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public UpdateVacationHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(UpdateVacationCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            // يمكن التعديل فقط إذا كانت في حالة pending
            if (vacation.Status != Common.Enums.VacationStatus.Pending)
                return ResponseModel.Fail("Can only update pending vacations", 400);

            // تحديث الحقول إذا كانت لها قيمة
            if (request.Type.HasValue)
                vacation.Type = request.Type.Value;

            if (request.StartDate.HasValue)
                vacation.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                vacation.EndDate = request.EndDate.Value;

            if (request.SubstituteId.HasValue)
                vacation.SubstituteId = request.SubstituteId.Value;

            if (!string.IsNullOrEmpty(request.Reason))
                vacation.Reason = request.Reason;

            // إعادة حساب عدد الأيام
            if (request.StartDate.HasValue || request.EndDate.HasValue)
            {
                vacation.Days = (int)(vacation.EndDate.Date - vacation.StartDate.Date).TotalDays + 1;
            }

            vacation.UpdatedOn = DateTime.UtcNow;

            await _vacationRepository.UpdateAsync(vacation);
            return ResponseModel.Success("Vacation updated successfully");
        }
    }
}