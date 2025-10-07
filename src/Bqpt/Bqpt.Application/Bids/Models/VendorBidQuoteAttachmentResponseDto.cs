using System.Collections.Generic;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorBidQuoteAttachmentResponseDto : BaseDto
    {
        public IEnumerable<VendorBidQuoteAttachmentDto> VendorBidQuoteAttachments { get; set; }
        public VendorBidQuoteAttachmentViewModel VendorBidQuoteAttachmentForm { get; set; }

        public string Message{ get; set; }
    }
}