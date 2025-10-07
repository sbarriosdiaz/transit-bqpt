using System;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorInPartDto : BaseDto
    {
        public string BidQuoteDomainKey { get; set; }

        public DateTime NotificationSent { get; set; } = DateTime.Now;

        public int BidQuotePartId { get; set; }

        public int VendorId { get; set; }
        public VendorDto Vendor { get; set; }

        public string AssetWorksPartNumber { get; set; }
        public int? RequestedQuantity { get; set; }
        public string AssetWorksPartDescription { get; set; }
    }
}