using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteCloseCommandValidator : AbstractValidator<BidQuoteCloseCommand>
    {
        public BidQuoteCloseCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}