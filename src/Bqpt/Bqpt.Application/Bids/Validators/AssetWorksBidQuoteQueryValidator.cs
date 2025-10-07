using FluentValidation;

namespace Bqpt.Application
{
    public class AssetWorksBidQuoteQueryValidator : AbstractValidator<AssetWorksBidQuoteQuery>
    {
        public AssetWorksBidQuoteQueryValidator()
        {
            RuleFor(p => p.AssetWorksBidQuoteId).NotEmpty().NotNull();
        }
    }
}