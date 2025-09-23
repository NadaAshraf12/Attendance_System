using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Common.Enums;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetVacationsByStatus
{
    public class GetVacationsByStatusHandler : IRequestHandler<GetVacationsByStatusQuery, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public GetVacationsByStatusHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(GetVacationsByStatusQuery request, CancellationToken cancellationToken)
        {
            var vacations = await _vacationRepository.GetByStatusAsync(request.Status);

            var result = vacations.Select(v => new VacationDto
            {
                Id = v.Id,
                UserFullName = v.User.FullName,
                DepartmentName = v.User.Department?.Name,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Days = v.Days,
                Status = v.Status.ToString()
            }).ToList();

            return ResponseModel.Success("Vacations retrieved successfully", result);
        }
    }
}
