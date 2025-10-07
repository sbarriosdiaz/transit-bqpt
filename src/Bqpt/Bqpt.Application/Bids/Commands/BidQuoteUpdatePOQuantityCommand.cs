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
    public class BidQuoteUpdatePOQuantityCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteUpdatePOQuantityCommand(BidQuotePartViewModel form) => Form = form;

        public BidQuotePartViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteUpdatePOQuantityCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<TransactionResult<string>> Handle(BidQuoteUpdatePOQuantityCommand request, CancellationToken cancellationToken)
            {
                var bidQuotePart = await _dbService.Set<BidQuotePart>()
                                                   .Where(b => b.DomainKey.Equals(request.Form.DomainKey)
                                                          && b.BidQuote.Status.Equals(nameof(BidQuoteStatus.Ended)))
                                                   .FirstOrDefaultAsync(cancellationToken);

                if (bidQuotePart is null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                bidQuotePart.PurchaseOrderQuantity = request.Form.PurchaseOrderQuantity;

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuotePart.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}