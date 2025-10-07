using FluentValidation;

namespace Bqpt.Application
{
    public class VendorUpdateBidCommandValidator : AbstractValidator<VendorUpdateBidCommand>
    {
        public VendorUpdateBidCommandValidator()
        {
            RuleFor(p => p.Form.EstimateDeliveryDate).NotNull().NotEmpty();
            RuleFor(p => p.Form.QuantityAvailable).NotNull().NotEmpty();
            RuleFor(p => p.Form.UnitPrice).NotNull().NotEmpty();
            RuleFor(p => p.Form.ItemsPerUnit).NotNull().NotEmpty();
            RuleFor(p => p.Form.DomainKey).NotNull().NotEmpty();
        }
    }
}