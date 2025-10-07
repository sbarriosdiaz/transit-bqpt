using System.Collections.Generic;

namespace Bqpt.Application
{
    public class BidQuoteDashboardResponseDto
    {
        public IEnumerable<AssetWorksBidQuoteDto> AssetWorksBidQuotes { get; set; }

        public IEnumerable<BidQuoteDto> OpenBidQuotes { get; set; }

        public IEnumerable<BidQuoteDto> ClosedBidQuotes { get; set; }
    }
}