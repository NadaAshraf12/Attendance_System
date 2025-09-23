using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationById
{
    public class GetVacationByIdHandler : IRequestHandler<GetVacationByIdQuery, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public GetVacationByIdHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(GetVacationByIdQuery request, CancellationToken cancellationToken)
        {
            var vacation = await _vacationRepository.GetByIdWithUserAsync(request.VacationId);

            if (vacation == null)
                return ResponseModel.Fail("Vacation not found", 404);

            // Fix: Pass the message as the first argument and the vacation object as the second argument
            return ResponseModel.Success("Vacation retrieved successfully", vacation);
        }
    }
}