using FluentValidation;
using TournamentApi.Dtos.Games;

namespace TournamentApi.Validators.Games
{
    public class PatchGameRequestValidator : AbstractValidator<PatchGameRequest>
    {
        public PatchGameRequestValidator()
        {
            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title cannot be empty.")
                    .MinimumLength(3).WithMessage("Title must be at least 3 characters long.");
            });

            When(x => x.Time != null, () =>
            {
                RuleFor(x => x.Time!.Value)
                    .Must(time => time >= DateTime.UtcNow)
                    .WithMessage("Game time cannot be in the past.");
            });

            When(x => x.TournamentId != null, () =>
            {
                RuleFor(x => x.TournamentId!.Value)
                    .GreaterThan(0).WithMessage("TournamentId must be a valid ID.");
            });
        }
    }
}
