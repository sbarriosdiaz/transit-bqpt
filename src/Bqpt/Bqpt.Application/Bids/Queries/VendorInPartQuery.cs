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
    public class VendorInPartQuery : IRequest<VendorPartBidViewModel>
    {
        public VendorInPartQuery(string domainKey) => DomainKey = domainKey;

        public string DomainKey { get; }

        public class Handler : IRequestHandler<VendorInPartQuery, VendorPartBidViewModel>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<VendorPartBidViewModel> Handle(VendorInPartQuery request, CancellationToken cancellationToken) => await _dbService.Set<VendorInPart>()
                                                .AsNoTracking()
                                                .Include(x => x.BidQuotePart.BidQuote)
                                                .Where(x => x.BidQuotePart.DomainKey.Equals(request.DomainKey))
                                                .Select(vp => new VendorPartBidViewModel
                                                {
                                                    DomainKey = vp.DomainKey,
                                                    Id = vp.Id,
                                                    BidQuoteDomainKey = vp.BidQuotePart.BidQuote.DomainKey,
                                                    BidQuotePartId = vp.BidQuotePartId,
                                                    VendorId = vp.VendorId,
                                                    RequestedQuantity = vp.BidQuotePart.RequestedQuantity,
                                                    AssetWorksPartNumber = vp.BidQuotePart.AssetWorksPartNumber,
                                                    AssetWorksPartDescription = vp.BidQuotePart.AssetWorksPartDescription,
                                                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}