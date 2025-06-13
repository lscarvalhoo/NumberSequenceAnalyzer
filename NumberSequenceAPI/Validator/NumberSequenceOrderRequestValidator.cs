using Application.Models.Request;
using FluentValidation;

namespace API.Validator
{
    public class NumberSequenceOrderRequestValidator : AbstractValidator<NumberSequenceOrderRequest>
    {
        public NumberSequenceOrderRequestValidator()
        {
            RuleFor(x => x.Values)
                .NotNull().WithMessage("Values cannot be null.")
                .NotEmpty().WithMessage("Values cannot be empty.");
        }
    }
}
