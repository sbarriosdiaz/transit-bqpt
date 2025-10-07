using System;
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
    public class BidQuoteEndCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteEndCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteEndCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            private async Task SelectDefaultBidWinner(int Id, CancellationToken cancellationToken)
            {
                var bidQuoteParts = await _dbService.Set<BidQuotePart>()
                                                    .Include(p => p.Bids)
                                                    .Where(b => b.BidQuoteId == Id)
                                                    .ToListAsync(cancellationToken);

                VendorPartBid bid;
                foreach (var (part, lowestBid) in from part in bidQuoteParts
                                                  let lowestBid = part.Bids.Any() ? part.Bids.OrderBy(b => b.UnitPrice ?? 0).Select(b => b.Id).First() : 0
                                                  select (part, lowestBid))
                {
                    if (lowestBid > 0)
                    {
                        bid = await _dbService.Set<VendorPartBid>().Where(b => b.Id.Equals(lowestBid)).FirstOrDefaultAsync(cancellationToken);

                        bid.IsSelected = bid != null;
                    }

                    foreach (var bidpart in part.Bids)
                    {
                        if (bidpart.Id != lowestBid)
                        {
                            bidpart.IsSelected = false;
                        }
                    }
                }

                await _dbService.SaveChangesAsync(cancellationToken);
            }

            public async Task<TransactionResult<string>> Handle(BidQuoteEndCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(b => b.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote == null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                await SelectDefaultBidWinner(bidQuote.Id, cancellationToken);

                bidQuote.BidEndTime = DateTime.Now;
                bidQuote.SetStatus = BidQuoteStatus.Ended;

                bidQuote.StatusHistories.Add(new BidQuoteStatusHistory
                {
                    Status = bidQuote.Status
                });

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}