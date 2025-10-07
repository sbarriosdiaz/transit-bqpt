using System;
using System.Collections.Generic;
using Bqpt.Common;

namespace Bqpt.Domain
{
    public class BidQuote : Entity<int>
    {
        public string AssetWorksBidQuoteId { get; set; }
        public DateTime? AssetWorksDateInserted { get; set; }
        public DateTime? AssetWorksDateApproval { get; set; }
        public DateTime? AssetWorksDateRequired { get; set; }
        public DateTime? AssetWorksDateRequested { get; set; }

        public decimal? AssetWorksTotalCost { get; set; }

        public DateTime? BidScheduledStartTime { get; set; }
        public DateTime? BidScheduledEndTime { get; set; }
        public DateTime? BidStartTime { get; set; }
        public DateTime? BidEndTime { get; set; }

        public DateTime? BidImportedTime { get; set; }
        public DateTime? BidClosedTime { get; set; }

        public string Status
        {
            get { return SetStatus.ToString(); }
            private set { SetStatus = value.ParseEnum<BidQuoteStatus>(); }
        }

        public BidQuoteStatus SetStatus { get; set; }

        public ICollection<BidQuotePart> Parts { get; set; } = new HashSet<BidQuotePart>();
        public ICollection<BidQuoteStatusHistory> StatusHistories { get; set; } = new HashSet<BidQuoteStatusHistory>();
        public ICollection<BidQuoteNote> Notes { get; set; } = new HashSet<BidQuoteNote>();
        public ICollection<VendorBidQuoteAttachment> VendorBidQuoteAttachments { get; set; } = new HashSet<VendorBidQuoteAttachment>();
    }
}