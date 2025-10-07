using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteStartCommandValidator : AbstractValidator<BidQuoteStartCommand>
    {
        public BidQuoteStartCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}