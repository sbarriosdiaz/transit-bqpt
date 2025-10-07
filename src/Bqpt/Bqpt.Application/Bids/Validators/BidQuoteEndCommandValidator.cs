using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteEndCommandValidator : AbstractValidator<BidQuoteEndCommand>
    {
        public BidQuoteEndCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}