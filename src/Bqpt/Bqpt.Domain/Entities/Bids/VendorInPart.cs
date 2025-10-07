using System;

namespace Bqpt.Domain
{
    public class VendorInPart : Entity<int>
    {
        public DateTime NotificationSent { get; set; } = DateTime.Now;

        public int BidQuotePartId { get; set; }
        public BidQuotePart BidQuotePart { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}