using FluentValidation;
using TournamentApi.Dtos.Games;

namespace TournamentApi.Validators.Games
{
    public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
    {
        public CreateGameRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.");

            RuleFor(x => x.Time)
                .Must(time => time >= DateTime.UtcNow)
                .WithMessage("Game time cannot be in the past.");

            RuleFor(x => x.TournamentId)
                .GreaterThan(0).WithMessage("TournamentId must be a valid ID.");
        }
    }
}
