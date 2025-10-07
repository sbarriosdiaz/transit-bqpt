using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteImportCommandValidator : AbstractValidator<BidQuoteImportCommand>
    {
        public BidQuoteImportCommandValidator()
        {
            RuleFor(p => p.Form.AssetWorksBidQuoteId).NotNull().NotEmpty();
        }
    }
}