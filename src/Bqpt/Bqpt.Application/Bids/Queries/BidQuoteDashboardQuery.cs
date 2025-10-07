////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    public class BidQuoteDashboardQuery : IRequest<BidQuoteDashboardResponseDto>

    {
        public class Handler : IRequestHandler<BidQuoteDashboardQuery, BidQuoteDashboardResponseDto>
        {
            private readonly IDbService _dbService;
            private readonly IAssetWorksServices _awService;

            public Handler(IDbService dbService, IAssetWorksServices awService)
            {
                _dbService = dbService;
                _awService = awService;
            }

            public async Task<BidQuoteDashboardResponseDto> Handle(BidQuoteDashboardQuery request, CancellationToken cancellationToken)
            {
                var awBidQuotes = await _awService.GetAssetWorksBidQuotes();

                var bidQuotes = await _dbService.Set<BidQuote>()
                                                .AsNoTracking()
                                                .Include(x => x.Parts)
                                                .Include(x => x.Parts.Select(b => b.Bids))
                                                .Where(x => x.IsActive)
                                                .OrderByDescending(x => x.CreatedOn)
                                                .Select(bq => new BidQuoteDto
                                                {
                                                    DomainKey = bq.DomainKey,
                                                    AssetWorksBidQuoteId = bq.AssetWorksBidQuoteId,
                                                    AssetWorksDateInserted = bq.AssetWorksDateInserted,
                                                    AssetWorksDateApproval = bq.AssetWorksDateApproval,
                                                    AssetWorksDateRequired = bq.AssetWorksDateRequired,
                                                    AssetWorksDateRequested = bq.AssetWorksDateRequested,
                                                    Status = (bq.BidScheduledEndTime < DateTime.Now && bq.BidEndTime == null)
                                                                ? nameof(BidQuoteStatus.Expired)
                                                                : bq.Status,
                                                    BidStartTime = bq.BidStartTime,
                                                    BidEndTime = bq.BidEndTime,
                                                    BidScheduledEndTime = bq.BidScheduledEndTime,
                                                    BidImportedTime = bq.BidImportedTime,
                                                    BidClosedTime = bq.BidClosedTime,
                                                    Parts = bq.Parts.Where(p => p.IsActive)
                                                    .Select(p => new BidQuotePartDto
                                                    {
                                                        AssetWorksPartNumber = p.AssetWorksPartNumber,
                                                        VendorBids = p.Bids.Where(b => b.IsActive)
                                                        .Select(b => new VendorPartBidDto
                                                        {
                                                            UnitPrice = b.UnitPrice
                                                        }).ToList()
                                                    }).ToList()
                                                }).ToListAsync(cancellationToken);

                return new BidQuoteDashboardResponseDto
                {
                    OpenBidQuotes = bidQuotes.Where(b => b.Status.Equals(nameof(BidQuoteStatus.Open))
                                                        || b.Status.Equals(nameof(BidQuoteStatus.New)))
                                             .Take(10),
                    ClosedBidQuotes = bidQuotes.Where(b => b.Status.Equals(nameof(BidQuoteStatus.Closed))
                                                        || b.Status.Equals(nameof(BidQuoteStatus.Expired))
                                                        || b.Status.Equals(nameof(BidQuoteStatus.Ended)))
                                                .Take(10),
                    AssetWorksBidQuotes = bidQuotes.Any()
                                            ? awBidQuotes.OrderByDescending(c => c.AssetWorksDateRequested)
                                                
                                                .Where(p => !bidQuotes.Any(d => d.AssetWorksBidQuoteId.Equals(p.AssetWorksBidQuoteId)))
                                                .Select(p => new AssetWorksBidQuoteDto
                                                {
                                                    AssetWorksBidQuoteId = p.AssetWorksBidQuoteId,
                                                    AssetWorksDateInserted = p.AssetWorksDateInserted,
                                                    AssetWorksDateApproval = p.AssetWorksDateApproval,
                                                    AssetWorksDateRequired = p.AssetWorksDateRequired,
                                                    AssetWorksDateRequested = p.AssetWorksDateRequested,
                                                    AssetWorksPartCount = p.AssetWorksPartCount,
                                                    AssetWorksPartCountWithNoCC = p.AssetWorksPartCountWithNoCC,
                                                    AssetWorksNoVendorCCMatch = p.AssetWorksNoVendorCCMatch,
                                                    AssetWorksVendorsNoEmail = p.AssetWorksVendorsNoEmail
                                                }).OrderByDescending(p => p.AssetWorksBidQuoteId)
                                                .Take(25)
                                           : awBidQuotes.OrderByDescending(c => c.AssetWorksDateRequested)
                };
            }
        }
    }
}