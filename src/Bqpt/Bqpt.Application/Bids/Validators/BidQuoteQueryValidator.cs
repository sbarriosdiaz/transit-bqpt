using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteQueryValidator : AbstractValidator<BidQuoteQuery>
    {
        public BidQuoteQueryValidator()
        {
            RuleFor(p => p.DomainKey).NotEmpty().NotNull();
        }
    }
}