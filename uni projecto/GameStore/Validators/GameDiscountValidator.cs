using FluentValidation;

namespace GameStore.Validators
{
    public class GameDiscountValidator : AbstractValidator<decimal>
    {
        public GameDiscountValidator()
        {
            RuleFor(x => x)
                .InclusiveBetween(0, 100)
                .WithMessage("Discount percentage must be between 0 and 100.");
        }
    }
}
