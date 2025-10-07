using FluentValidation;

namespace Bqpt.Application
{
    public class VendorInPartQueryValidator : AbstractValidator<VendorInPartQuery>
    {
        public VendorInPartQueryValidator()
        {
            RuleFor(p => p.DomainKey).NotEmpty().NotNull();
        }
    }
}