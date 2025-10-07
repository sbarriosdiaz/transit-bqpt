////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteSelectBidderCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteSelectBidderCommand(VendorPartBidSelectedViewModel form) => Form = form;

        public VendorPartBidSelectedViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteSelectBidderCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<TransactionResult<string>> Handle(BidQuoteSelectBidderCommand request, CancellationToken cancellationToken)
            {
                var vendorPartBid = await _dbService.Set<VendorPartBid>()
                                                    .Where(b => b.DomainKey.Equals(request.Form.DomainKey)
                                                           && b.BidQuotePartId.Equals(request.Form.BidQuotePartId ?? 0)
                                                           && b.BidQuotePart.BidQuote.Status.Equals(nameof(BidQuoteStatus.Ended)))
                                                    .FirstOrDefaultAsync(cancellationToken);

                if (vendorPartBid is null) return new TransactionResult<string>("Error", "Error: Bid does not exist");

                var vendorPartBids = await _dbService.Set<VendorPartBid>().Where(b => b.BidQuotePartId == vendorPartBid.BidQuotePartId).ToListAsync(cancellationToken);

                foreach (var v in vendorPartBids.Where(x => x.Id != vendorPartBid.Id))
                {
                    v.IsSelected = false;
                }

                vendorPartBid.IsSelected = !vendorPartBid.IsSelected ?? false;

                if (vendorPartBid.IsSelected ?? false)
                {
                    vendorPartBid.VendorPartBidSelectedHistories.Add(new VendorPartBidSelectedHistory
                    {
                        VendorPartBidId = vendorPartBid.Id
                    });
                }

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(vendorPartBid.IsSelected.ToString(), AppConstants.TransactionSuccess);
            }
        }
    }
}