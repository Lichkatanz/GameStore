using FluentValidation;
using GameStore.Controllers;

namespace GameStore.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.GameId)
                .NotEmpty().WithMessage("GameId is required.")
                .Matches("^[a-fA-F0-9]{24}$").WithMessage("Invalid GameId format (must be a 24-character hex string).");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MinimumLength(3).WithMessage("Customer name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Customer name must not exceed 50 characters.");
        }
    }
}
