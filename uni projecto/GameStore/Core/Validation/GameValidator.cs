using FluentValidation;
using GameStore.Core.Models;

namespace GameStore.Core.Validation
{
    public class GameValidator : AbstractValidator<GameDto>
    {
        public GameValidator()
        {
            RuleFor(g => g.Name).NotEmpty().MaximumLength(100);
            RuleFor(g => g.Price).GreaterThan(0);
            RuleFor(g => g.Developer).NotEmpty().MaximumLength(100);
        }
    }
}