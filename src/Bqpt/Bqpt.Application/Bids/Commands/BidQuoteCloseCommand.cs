using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteCloseCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteCloseCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteCloseCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<TransactionResult<string>> Handle(BidQuoteCloseCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(b => b.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote == null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                bidQuote.BidClosedTime = DateTime.Now;
                bidQuote.SetStatus = BidQuoteStatus.Closed;

                bidQuote.StatusHistories.Add(new BidQuoteStatusHistory
                {
                    BidQuote = bidQuote,
                    Status = bidQuote.Status
                });

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}