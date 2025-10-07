using System;
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
    public class VendorDashboardQuery : IRequest<VendorDashboardResponseDto>

    {
        public class Handler : IRequestHandler<VendorDashboardQuery, VendorDashboardResponseDto>
        {
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IDbService dbService, ICurrentUserService currentUserService)
            {
                _dbService = dbService;
                _currentUserService = currentUserService;
            }

            public async Task<VendorDashboardResponseDto> Handle(VendorDashboardQuery request, CancellationToken cancellationToken)
            {
                var vendorEmail = _currentUserService.UserName.ToLowerInvariant();

                var vendor = await _dbService.Set<Vendor>().AsNoTracking()
                                    .Where(v => (v.AssetWorksVendorEmail.Equals(vendorEmail))
                                    || (v.Contacts.Select(c => c.AssetWorksContactEmail).Contains(vendorEmail)))
                                    .Select(v => new VendorDto
                                    {
                                        AssetWorksVendorName = v.AssetWorksVendorName
                                    })
                                    .FirstOrDefaultAsync(cancellationToken);

                var vendorParts = await _dbService.Set<VendorInPart>().AsNoTracking()
                                .Include(vp => vp.BidQuotePart.BidQuote)
                                .Where(vp => (vp.Vendor.AssetWorksVendorEmail.Equals(vendorEmail))
                                || (vp.Vendor.Contacts.Select(c => c.AssetWorksContactEmail).Contains(vendorEmail)))
                                .ToListAsync(cancellationToken);

                var vendorBidQuoteIds = vendorParts.Select(p => p.BidQuotePart.BidQuote.Id).Distinct().ToList();
                var vendorId = vendorParts.Select(p => p.VendorId).FirstOrDefault();

                var bidQuotes = await _dbService.Set<BidQuote>()
                                                .AsNoTracking()
                                                .Include(x => x.Parts)
                                                .Include(x => x.Parts.Select(b => b.Bids))
                                                .Include(x => x.Parts.Select(b => b.VendorInParts))
                                                .Where(x => x.IsActive.Equals(true)
                                                   && !x.Status.Equals(nameof(BidQuoteStatus.New))
                                                   && vendorBidQuoteIds.Any() && vendorBidQuoteIds.Contains(x.Id))
                                                .OrderByDescending(x => x.Status)
                                                .ThenBy(x => x.BidEndTime ?? x.BidScheduledEndTime)
                                                .Select(bq => new BidQuoteDto
                                                {
                                                    DomainKey = bq.DomainKey,
                                                    AssetWorksBidQuoteId = bq.AssetWorksBidQuoteId,
                                                    Status = (bq.BidScheduledEndTime < DateTime.Now && bq.BidEndTime.Equals(null)) ? (nameof(BidQuoteStatus.Expired)) : bq.Status,
                                                    BidStartTime = bq.BidStartTime,
                                                    BidEndTime = bq.BidEndTime ?? bq.BidScheduledEndTime,
                                                    BidScheduledEndTime = bq.BidScheduledEndTime,
                                                    VendorId = bq.Parts.SelectMany(p => p.VendorInParts.Select(v => v.VendorId)).FirstOrDefault(),

                                                    Parts = bq.Parts.Where(p => p.IsActive.Equals(true)).Select(p => new BidQuotePartDto
                                                    {
                                                        AssetWorksPartNumber = p.AssetWorksPartNumber,
                                                        RequestedQuantity = p.RequestedQuantity,
                                                        VendorInParts = p.VendorInParts.Where(b => b.IsActive.Equals(true) && b.VendorId == vendorId).Select(b => new VendorInPartDto
                                                        {
                                                            VendorId = b.VendorId
                                                        }).ToList(),

                                                        VendorBids = p.Bids.Where(b => b.IsActive.Equals(true) && b.VendorId == vendorId).Select(b => new VendorPartBidDto
                                                        {
                                                            VendorId = b.VendorId,
                                                            IsSelected = b.IsSelected
                                                        }).ToList()
                                                    }).ToList()
                                                }).ToListAsync(cancellationToken);

                return new VendorDashboardResponseDto { BidQuotes = bidQuotes, Vendor = vendor };
            }
        }
    }
}