////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    public class BidQuoteAllDashboardQuery : IRequest<BidQuoteAllDashboardResponseDto>

    {
        public class Handler : IRequestHandler<BidQuoteAllDashboardQuery, BidQuoteAllDashboardResponseDto>
        {
            private readonly IDbService _dbService;
            private readonly IAssetWorksServices _awService;

            public Handler(IDbService dbService, IAssetWorksServices awService)
            {
                _dbService = dbService;
                _awService = awService;
            }

            public async Task<BidQuoteAllDashboardResponseDto> Handle(BidQuoteAllDashboardQuery request, CancellationToken cancellationToken)
            {
                var awBidQuotes = await _awService.GetAssetWorksBidQuotes();

                var bidquotes = await _dbService.Set<BidQuote>()
                                                .AsNoTracking()

                                                .OrderByDescending(x => x.Status)
                                                .Select(bq => new BidQuoteDto
                                                {
                                                    DomainKey = bq.DomainKey,
                                                    AssetWorksBidQuoteId = bq.AssetWorksBidQuoteId,
                                                    AssetWorksDateInserted = bq.AssetWorksDateInserted,
                                                    AssetWorksDateApproval = bq.AssetWorksDateApproval,
                                                    AssetWorksDateRequired = bq.AssetWorksDateRequired,
                                                    AssetWorksDateRequested = bq.AssetWorksDateRequested,
                                                    Status = bq.Status,
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

                return new BidQuoteAllDashboardResponseDto
                {
                    AllBidQuotes = bidquotes.ToList(),

                    ClosedBidQuotes = bidquotes.Where(b => b.Status.Equals(nameof(BidQuoteStatus.Closed))
                                                        || b.Status.Equals(nameof(BidQuoteStatus.Expired))
                                                        || b.Status.Equals(nameof(BidQuoteStatus.Ended)))
                                                .ToList(),

                    AssetWorksBidQuotes = bidquotes.Any()
                                            ? awBidQuotes.OrderByDescending(c => c.AssetWorksDateRequested)
                                                .Where(p => !bidquotes.Any(d => d.AssetWorksBidQuoteId.Equals(p.AssetWorksBidQuoteId)))
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
                                                })
                                                .ToList()
                                           : awBidQuotes.OrderByDescending(c => c.AssetWorksDateRequested).Take(20)
                };
            }
        }
    }
}