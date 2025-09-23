using FluentValidation;

namespace CleanArch.App.Features.Vacation.Command.CreateVacation
{
    public class CreateVacationCommandValidator : AbstractValidator<CreateVacationCommand>
    {
        public CreateVacationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .GreaterThan(DateTime.Now.Date).WithMessage("Start date must be in the future");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
        }
    }
}