using System;
using System.Collections.Generic;

namespace Bqpt.Domain
{
    public class VendorPartBid : Entity<int>
    {
        public int? QuantityAvailable { get; set; }
        public int? ItemsPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }

        public decimal? Core { get; set; }
        public DateTime? EstimateDeliveryDate { get; set; }
        public string Comment { get; set; }

        public bool? IsSelected { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int BidQuotePartId { get; set; }
        public BidQuotePart BidQuotePart { get; set; }

        public ICollection<VendorPartBidHistory> VendorPartBidHistories { get; set; } = new HashSet<VendorPartBidHistory>();
        public ICollection<VendorPartBidSelectedHistory> VendorPartBidSelectedHistories { get; set; } = new HashSet<VendorPartBidSelectedHistory>();
    }
}