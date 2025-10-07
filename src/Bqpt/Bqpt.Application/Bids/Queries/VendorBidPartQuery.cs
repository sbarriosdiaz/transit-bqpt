using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class VendorBidPartQuery : IRequest<VendorPartBidViewModel>
    {
        public VendorBidPartQuery(string domainKey)
        {
            DomainKey = domainKey;
        }

        public string DomainKey { get; }

        public class Handler : IRequestHandler<VendorBidPartQuery, VendorPartBidViewModel>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService)
            {
                _dbService = dbService;
            }

            public async Task<VendorPartBidViewModel> Handle(VendorBidPartQuery request, CancellationToken cancellationToken)
            {
                var bidQuote = await _dbService.Set<VendorPartBid>()
                                                .AsNoTracking()
                                                .Include(x => x.BidQuotePart)
                                                .Include(x => x.BidQuotePart.BidQuote)
                                                .Where(x => x.DomainKey.Equals(request.DomainKey))
                                                .Select(vp => new VendorPartBidViewModel
                                                {
                                                    DomainKey = vp.DomainKey,
                                                    BidQuoteDomainKey = vp.BidQuotePart.BidQuote.DomainKey,
                                                    BidQuotePartId = vp.BidQuotePartId,
                                                    VendorId = vp.VendorId,
                                                    RequestedQuantity = vp.BidQuotePart.RequestedQuantity,
                                                    AssetWorksPartNumber = vp.BidQuotePart.AssetWorksPartNumber,
                                                    AssetWorksPartDescription = vp.BidQuotePart.AssetWorksPartDescription,
                                                    EstimateDeliveryDate = vp.EstimateDeliveryDate,
                                                    QuantityAvailable = vp.QuantityAvailable,
                                                    ItemsPerUnit = vp.ItemsPerUnit,
                                                    UnitPrice = vp.UnitPrice,
                                                    Core=vp.Core,
                                                    Comment = vp.Comment
                                                }).FirstOrDefaultAsync(cancellationToken);

                return bidQuote;
            }
        }
    }
}