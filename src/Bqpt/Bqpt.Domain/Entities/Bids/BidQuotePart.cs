using System;
using System.Collections.Generic;

namespace Bqpt.Domain
{
    public class BidQuotePart : Entity<int>
    {
        public string AssetWorksPartNumber { get; set; }
        public string AssetWorksPartCommodityCode { get; set; }

        public string AssetWorksPartManufacturerPartNumber { get; set; }
        public string AssetWorksPartSuffix { get; set; }
        public string AssetWorksPartDescription { get; set; }
        public string AssetWorksUnitPrice { get; set; }

        public string AssetWorksPartCategory { get; set; }
        public string AssetWorksPartLineNumber { get; set; }
        public string AssetWorksMinPartLineNumber { get; set; }
        public int AssetWorksMinPartLineNumberInt { get; set; }        
        public string AssetWorksPartLocationCode { get; set; }

        public DateTime? AssetWorksPartDateInserted { get; set; }
        public DateTime? AssetWorksPartDateRequired { get; set; }

        public int? AssetWorksPartQuantityRequested { get; set; }
        public int? AssetWorksPartQuantityOnHand { get; set; }
        public int? AssetWorksPartQuantityOnOrder { get; set; }
        public int? AssetWorksPartQuantityCommited { get; set; }

        public int? RequestedQuantity { get; set; }
        public int? PurchaseOrderQuantity { get; set; }

        public int BidQuoteId { get; set; }
        public BidQuote BidQuote { get; set; }

        public ICollection<VendorInPart> VendorInParts { get; set; } = new HashSet<VendorInPart>();

        public ICollection<VendorPartBid> Bids { get; set; } = new HashSet<VendorPartBid>();
    }
}