using FluentValidation;

namespace Bqpt.Application
{
    public class VendorAddBidCommandValidator : AbstractValidator<VendorAddBidCommand>
    {
        public VendorAddBidCommandValidator()
        {
            RuleFor(p => p.Form.EstimateDeliveryDate).NotNull().NotEmpty();
            RuleFor(p => p.Form.QuantityAvailable).NotNull().NotEmpty();
            RuleFor(p => p.Form.UnitPrice).NotNull().NotEmpty();
            RuleFor(p => p.Form.ItemsPerUnit).NotNull().NotEmpty();
            RuleFor(p => p.Form.VendorId).NotNull().NotEmpty();
            RuleFor(p => p.Form.BidQuotePartId).NotNull().NotEmpty();
        }
    }
}