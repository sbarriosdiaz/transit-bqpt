using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteDeleteCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteDeleteCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteDeleteCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService, IAssetWorksServices awService)
            {
                _dbService = dbService;
            }

            public async Task<TransactionResult<string>> Handle(BidQuoteDeleteCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();

                var bidQuote = await _dbService.Set<BidQuote>()
                                               .Include(q => q.Parts.Select(v => v.VendorInParts))
                                               .Include(q => q.Parts.Select(v => v.Bids))
                                               .Include(q => q.VendorBidQuoteAttachments)
                                               .Include(q => q.StatusHistories)
                                               .FirstOrDefaultAsync(q => q.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote == null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                _dbService.Set<VendorInPart>().RemoveRange(bidQuote.Parts.SelectMany(p => p.VendorInParts));
                _dbService.Set<VendorPartBid>().RemoveRange(bidQuote.Parts.SelectMany(p => p.Bids));
                _dbService.Set<VendorBidQuoteAttachment>().RemoveRange(bidQuote.VendorBidQuoteAttachments);
                _dbService.Set<BidQuotePart>().RemoveRange(bidQuote.Parts);
                _dbService.Set<BidQuoteNote>().RemoveRange(bidQuote.Notes);
                _dbService.Set<BidQuoteStatusHistory>().RemoveRange(bidQuote.StatusHistories);
                _dbService.Set<BidQuote>().Remove(bidQuote);

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}