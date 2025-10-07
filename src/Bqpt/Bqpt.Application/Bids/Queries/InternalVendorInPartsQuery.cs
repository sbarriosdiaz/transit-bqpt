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
    public class InternalVendorInPartsQuery : IRequest<BidQuoteDto>
    {
        public InternalVendorInPartsQuery(string domainKey) => DomainKey = domainKey;

        public string DomainKey { get; }

        public class Handler : IRequestHandler<VendorInPartsQuery, BidQuoteDto>
        {
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IDbService dbService, ICurrentUserService currentUserService)
            {
                _dbService = dbService;
                _currentUserService = currentUserService;
            }

            public async Task<BidQuoteDto> Handle(VendorInPartsQuery request, CancellationToken cancellationToken)
            {
                var bidQuote = await _dbService.Set<BidQuote>()
                                                .AsNoTracking()
                                                .Include(x => x.Parts.Select(b => b.Bids))
                                                .Include(x => x.Parts.Select(b => b.VendorInParts))
                                                .Where(x => x.DomainKey.Equals(request.DomainKey)
                                                && !x.Status.Equals(nameof(BidQuoteStatus.New)))
                                                .OrderByDescending(x => x.Status)
                                                .ThenBy(x => x.BidEndTime ?? x.BidScheduledEndTime)
                                                .FirstOrDefaultAsync(cancellationToken);

                if (bidQuote is null) return null;

                var vendorEmail = _currentUserService.UserName.ToLowerInvariant();

                var vendor = await _dbService.Set<Vendor>().AsNoTracking()
                                .Where(v => (v.AssetWorksVendorEmail.Equals(vendorEmail))
                                || (v.Contacts.Select(c => c.AssetWorksContactEmail).Contains(vendorEmail)))
                                .FirstOrDefaultAsync(cancellationToken);

                if (vendor is null) return null;

                var vendorId = vendor.Id;

                var response = new BidQuoteDto
                {
                    DomainKey = bidQuote.DomainKey,
                    AssetWorksBidQuoteId = bidQuote.AssetWorksBidQuoteId,
                    Status = (bidQuote.BidScheduledEndTime < DateTime.Now && bidQuote.BidEndTime.Equals(null)) ? (nameof(BidQuoteStatus.Expired)) : bidQuote.Status,
                    BidStartTime = bidQuote.BidStartTime,
                    BidEndTime = bidQuote.BidEndTime ?? bidQuote.BidScheduledEndTime,
                    BidScheduledEndTime = bidQuote.BidScheduledEndTime,
                    BidClosedTime = bidQuote.BidClosedTime,
                    VendorId = vendorId,
                    VendorName = vendor.AssetWorksVendorName,

                    Parts = bidQuote.Parts.Where(p => p.VendorInParts.Where(vp => vp.VendorId == vendorId && vp.BidQuotePartId == p.Id).Select(bp => bp.BidQuotePart).Select(bp => new BidQuotePartDto
                    {
                        DomainKey = bp.DomainKey,
                        AssetWorksPartNumber = bp.AssetWorksPartNumber,
                        AssetWorksPartDescription = bp.AssetWorksPartDescription,
                        RequestedQuantity = bp.RequestedQuantity,

                        VendorInPart = p.VendorInParts.Where(vp => vp.VendorId == vendorId).Select(vp => new VendorInPartDto
                        {
                            DomainKey = vp.DomainKey,
                            VendorId = vp.VendorId
                        }).FirstOrDefault(),

                        VendorBid = p.Bids.Where(vb => vb.VendorId == vendorId).Select(vb => new VendorPartBidDto
                        {
                            DomainKey = vb.DomainKey,
                            EstimateDeliveryDate = vb.EstimateDeliveryDate,
                            QuantityAvailable = vb.QuantityAvailable,
                            ItemsPerUnit = vb.ItemsPerUnit,
                            UnitPrice = vb.UnitPrice,
                            Comment = vb.Comment,
                            VendorId = vb.VendorId,
                            IsSelected = vb.IsSelected
                        }).FirstOrDefault()
                    }).ToList()
                };

                response.CanUploadInvoice = response.Status.ParseEnum<BidQuoteStatus>() == BidQuoteStatus.Open && response.Parts.Any(p => p.VendorBid != null);

                return response;
            }
        }
    }
}