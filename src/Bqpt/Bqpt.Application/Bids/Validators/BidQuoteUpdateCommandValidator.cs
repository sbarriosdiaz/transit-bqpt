using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteUpdateCommandValidator : AbstractValidator<BidQuoteUpdateCommand>
    {
        public BidQuoteUpdateCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}