using FluentValidation;

namespace Bqpt.Application
{
    public class VendorBidPartQueryValidator : AbstractValidator<VendorBidPartQuery>
    {
        public VendorBidPartQueryValidator()
        {
            RuleFor(p => p.DomainKey).NotEmpty().NotNull();
        }
    }
}