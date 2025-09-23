using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Enums;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Repositories;
using MediatR;

namespace CleanArch.App.Features.Vacation.Command.CreateVacation
{
    public class CreateVacationHandler : IRequestHandler<CreateVacationCommand, ResponseModel>
    {
        private readonly IVacationRepository _vacationRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateVacationHandler(
            IVacationRepository vacationRepository,
            ICurrentUserService currentUserService)
        {
            _vacationRepository = vacationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseModel> Handle(CreateVacationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.StartDate >= request.EndDate)
                {
                    return ResponseModel.Fail("End date must be after start date", 400);
                }

                if (!Enum.IsDefined(typeof(VacationType), request.Type))
                {
                    return ResponseModel.Fail("Invalid vacation type", 400);
                }

                var vacation = new Domain.Entities.Vacation
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Type,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    SubstituteId = request.SubstituteId,
                    Reason = request.Reason,
                    AttachmentPath = request.AttachmentPath,
                    Status = VacationStatus.Pending,
                    CreatedOn = DateTime.UtcNow
                };



                var result = await _vacationRepository.AddAsync(vacation);

                return ResponseModel.Success("Vacation request created successfully", result);
            }
            catch (Exception ex)
            {
                return ResponseModel.Fail($"Error creating vacation: {ex.Message}");
            }
        }
    
    
    }
}