using CleanArch.App.Services;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetUserVacations
{
    public class GetUserVacationsHandler : IRequestHandler<GetUserVacationsQuery, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public GetUserVacationsHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(GetUserVacationsQuery request, CancellationToken cancellationToken)
        {
            var vacations = await _vacationRepository.GetByUserAsync(request.UserId);
            return ResponseModel.Success("User vacations retrieved successfully", vacations);
        }
    }
}