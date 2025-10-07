using System.Collections.Generic;

namespace Bqpt.Application
{
    public class BidQuoteAllDashboardResponseDto
    {
        public IEnumerable<AssetWorksBidQuoteDto> AssetWorksBidQuotes { get; set; }

        public IEnumerable<BidQuoteDto> AllBidQuotes { get; set; }

        public IEnumerable<BidQuoteDto> ClosedBidQuotes { get; set; }
    }
}