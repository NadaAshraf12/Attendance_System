using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Query.GetPendingVacations
{
    public class GetPendingVacationsHandler : IRequestHandler<GetPendingVacationsQuery, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;

        public GetPendingVacationsHandler(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<ResponseModel> Handle(GetPendingVacationsQuery request, CancellationToken cancellationToken)
        {
            var pendingVacations = await _vacationRepository.GetPendingForManagerAsync(request.ManagerId);

            var result = pendingVacations.Select(v => new VacationDto
            {
                Id = v.Id,
                UserFullName = v.User.FullName,
                DepartmentName = v.User.Department?.Name,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Days = v.Days,
                Status = v.Status.ToString()
            }).ToList();

            return ResponseModel.Success("Pending vacations retrieved successfully", result);

        }
    }
}