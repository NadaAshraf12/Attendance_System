using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Vacation.Command.ReviewVacationRequest
{
    public class ReviewVacationRequestHandler : IRequestHandler<ReviewVacationRequestCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public ReviewVacationRequestHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(ReviewVacationRequestCommand request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            if (vacation.Status != VacationStatus.Pending)
                return ResponseModel.Fail("Vacation is not pending review", 400);

            if (request.Status == VacationStatus.Accepted)
            {
                await _vacationRepository.ApproveAsync(vacation, request.ApproverId);
                return ResponseModel.Success("Vacation approved successfully");
            }
            else if (request.Status == VacationStatus.Declined)
            {
                await _vacationRepository.DeclineAsync(vacation, request.ApproverId, request.Reason);
                return ResponseModel.Success("Vacation declined successfully");
            }

            return ResponseModel.Fail("Invalid status. Only Accepted or Declined are allowed", 400);
        }
    }

}
