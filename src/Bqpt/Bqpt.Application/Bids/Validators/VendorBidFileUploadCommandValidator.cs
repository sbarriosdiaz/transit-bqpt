using FluentValidation;

namespace Bqpt.Application
{
    public class VendorBidFileUploadCommandValidator : AbstractValidator<VendorBidFileUploadCommand>
    {
        public VendorBidFileUploadCommandValidator()
        {
            RuleFor(p => p.Form.File).NotNull().NotEmpty();
            RuleFor(p => p.Form.BidQuoteId).NotNull().NotEmpty();
            RuleFor(p => p.Form.VendorId).NotNull().NotEmpty();
        }
    }
}