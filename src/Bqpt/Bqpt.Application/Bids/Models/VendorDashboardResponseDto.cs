using System.Collections.Generic;

namespace Bqpt.Application
{
    public class VendorDashboardResponseDto
    {
        public IEnumerable<BidQuoteDto> BidQuotes { get; set; }

        public VendorDto Vendor { get; set; }
    }
}