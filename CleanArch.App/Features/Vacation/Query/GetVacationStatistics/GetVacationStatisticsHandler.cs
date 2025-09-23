using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationStatistics
{
    public class GetVacationStatisticsHandler : IRequestHandler<GetVacationStatisticsQuery, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public GetVacationStatisticsHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(GetVacationStatisticsQuery request, CancellationToken cancellationToken)
        {
            var vacations = await _vacationRepository.GetByUserAsync(request.UserId);

            var yearVacations = vacations.Where(v => v.StartDate.Year == request.Year).ToList();

            var statistics = new
            {
                TotalVacations = yearVacations.Count,
                ApprovedVacations = yearVacations.Count(v => v.Status == VacationStatus.Accepted),
                PendingVacations = yearVacations.Count(v => v.Status == VacationStatus.Pending),
                DeclinedVacations = yearVacations.Count(v => v.Status == VacationStatus.Declined),
                TotalDays = yearVacations.Where(v => v.Status == VacationStatus.Accepted).Sum(v => v.Days),
                ByMonth = yearVacations
                    .GroupBy(v => v.StartDate.Month)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ByType = yearVacations
                    .GroupBy(v => v.Type)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count())
            };

            return ResponseModel.Success("Vacation statistics retrieved successfully", statistics);
        }
    }
}