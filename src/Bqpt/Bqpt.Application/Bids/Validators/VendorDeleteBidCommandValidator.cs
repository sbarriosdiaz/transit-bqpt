using FluentValidation;

namespace Bqpt.Application
{
    public class VendorDeleteBidCommandValidator : AbstractValidator<VendorDeleteBidCommand>
    {
        public VendorDeleteBidCommandValidator()
        {
            RuleFor(p => p.Form.DomainKey).NotNull().NotEmpty();
        }
    }
}