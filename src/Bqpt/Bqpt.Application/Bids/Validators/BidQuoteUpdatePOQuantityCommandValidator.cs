using FluentValidation;

namespace Bqpt.Application
{
    public class BidQuoteUpdatePOQuantityCommandValidator : AbstractValidator<BidQuoteUpdatePOQuantityCommand>
    {
        public BidQuoteUpdatePOQuantityCommandValidator()
        {
            RuleFor(p => p.Form.DomainKey).NotNull().NotEmpty();
            RuleFor(p => p.Form.PurchaseOrderQuantity).NotNull().NotEmpty();
        }
    }
}