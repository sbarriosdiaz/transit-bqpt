using System.Collections.Generic;

namespace Bqpt.Application
{
    public class BidQuoteResponseDto
    {
        public BidQuoteDto BidQuote { get; set; }

        public IEnumerable<VendorDto> SystemVendors { get; set; }
        
    }
}