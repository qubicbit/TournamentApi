using FluentValidation;
using TournamentApi.Dtos.Tournaments;

namespace TournamentApi.Validators.Tournaments
{
    public class CreateTournamentRequestValidator : AbstractValidator<CreateTournamentRequest>
    {
        public CreateTournamentRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.");

            RuleFor(x => x.Date)
                .Must(date => date >= DateTime.UtcNow)
                .WithMessage("Date cannot be in the past.");
        }
    }
}
