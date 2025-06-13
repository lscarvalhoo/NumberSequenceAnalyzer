using Application.Models.Request;
using FluentValidation;

namespace API.Validator
{
    internal sealed class NumberSequenceRequestValidator : AbstractValidator<NumberSequenceRequest>
    {
        public NumberSequenceRequestValidator()
        {
            RuleFor(x => x.Values)
                .NotNull().WithMessage("The 'valores' list cannot be null.")
                .NotEmpty().WithMessage("The 'valores' list cannot ser empty.");
        }
    }
}
