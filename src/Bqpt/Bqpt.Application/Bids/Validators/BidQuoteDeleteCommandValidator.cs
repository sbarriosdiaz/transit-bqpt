using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteDeleteCommandValidator : AbstractValidator<BidQuoteDeleteCommand>
    {
        public BidQuoteDeleteCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}