using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteSelectBidderCommandValidator : AbstractValidator<BidQuoteSelectBidderCommand>
    {
        public BidQuoteSelectBidderCommandValidator()
        {
            RuleFor(p => p.Form.DomainKey).NotNull().NotEmpty();
            RuleFor(p => p.Form.BidQuotePartId).NotNull().NotEmpty();
        }
    }
}