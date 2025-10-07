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
    public class VendorInPartsQuery : IRequest<BidQuoteDto>
    {
        public VendorInPartsQuery(string domainKey)
        {
            DomainKey = domainKey;
        }

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
                var bidQuoteParts = await _dbService.Set<BidQuotePart>().AsNoTracking()
                                    .Include(q => q.BidQuote)
                                    .Include(q => q.Bids)
                                    .Include(q => q.VendorInParts)
                                    .Where(q => q.BidQuote.DomainKey.Equals(request.DomainKey) &&
                                               !q.BidQuote.Status.Equals(nameof(BidQuoteStatus.New)))
                                    .OrderByDescending(x => x.BidQuote.BidEndTime ?? x.BidQuote.BidScheduledEndTime)
                                    .ToListAsync(cancellationToken);

                if (!bidQuoteParts.Any()) return null;

                var bidQuote = bidQuoteParts.FirstOrDefault().BidQuote;

                var vendorEmail = _currentUserService.UserName.ToLowerInvariant();

                var vendor = await _dbService.Set<Vendor>()
                                .AsNoTracking()
                                .Where(v => v.AssetWorksVendorEmail.Equals(vendorEmail) || v.Contacts.Select(c => c.AssetWorksContactEmail).Contains(vendorEmail))
                                .FirstOrDefaultAsync(cancellationToken);

                if (vendor is null) return null;

                var vendorId = vendor.Id;

                var vendorBidQuoteParts = bidQuoteParts.Select(p => p.VendorInParts.Where(vp => vp.VendorId == vendorId && vp.BidQuotePartId == p.Id)).SelectMany(bq => bq.Select(q => q.BidQuotePart)).ToList();
                var vendorInParts = vendorBidQuoteParts.SelectMany(p => p.VendorInParts).ToList();
                var vendorPartBids = vendorBidQuoteParts.SelectMany(p => p.Bids.Where(b => b.VendorId == vendorId)).ToList();

                var response = new BidQuoteDto
                {
                    DomainKey = bidQuote.DomainKey,
                    AssetWorksBidQuoteId = bidQuote.AssetWorksBidQuoteId,
                    Status = (bidQuote.BidScheduledEndTime < DateTime.Now && bidQuote.BidEndTime.Equals(null)) ? nameof(BidQuoteStatus.Expired) : bidQuote.Status,
                    BidStartTime = bidQuote.BidStartTime,
                    BidEndTime = bidQuote.BidEndTime ?? bidQuote.BidScheduledEndTime,
                    BidScheduledEndTime = bidQuote.BidScheduledEndTime,
                    BidClosedTime = bidQuote.BidClosedTime,
                    VendorId = vendorId,
                    VendorName = vendor.AssetWorksVendorName,
                    Parts = vendorBidQuoteParts.Select(bp => new BidQuotePartDto
                    {
                        DomainKey = bp.DomainKey,
                        AssetWorksPartNumber = bp.AssetWorksPartNumber,
                        AssetWorksPartDescription = bp.AssetWorksPartDescription,
                        RequestedQuantity = bp.RequestedQuantity,
                        VendorInPart = vendorInParts.Select(vp => new VendorInPartDto
                        {
                            DomainKey = bp.DomainKey,
                            VendorId = vp.VendorId
                        }).FirstOrDefault(),
                        VendorBid = vendorPartBids.Where(v => v.BidQuotePartId == bp.Id).Select(vb => new VendorPartBidDto
                        {
                            DomainKey = vb.DomainKey,
                            EstimateDeliveryDate = vb.EstimateDeliveryDate,
                            QuantityAvailable = vb.QuantityAvailable,
                            ItemsPerUnit = vb.ItemsPerUnit,
                            UnitPrice = vb.UnitPrice,
                            Core=vb.Core,
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