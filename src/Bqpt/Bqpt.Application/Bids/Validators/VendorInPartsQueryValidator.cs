using FluentValidation;

namespace Bqpt.Application
{
    public class VendorInPartsQueryValidator : AbstractValidator<VendorInPartsQuery>
    {
        public VendorInPartsQueryValidator()
        {
            RuleFor(p => p.DomainKey).NotEmpty().NotNull();
        }
    }
}