using System;
using System.Collections.Generic;
using System.Linq;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class BidQuotePartDto : BaseDto
    {
        public string AssetWorksPartNumber { get; set; }
        public string AssetWorksPartCommodityCode { get; set; }
        public string AssetWorksPartManufacturerPartNumber { get; set; }
        public string AssetWorksPartSuffix { get; set; }
        public string AssetWorksPartDescription { get; set; }
        public string AssetWorksPartCategory { get; set; }
        public string AssetWorksPartLineNumber { get; set; }
        public string AssetWorksMinPartLineNumber { get; set; }
        public int AssetWorksMinPartLineNumberInt { get; set; }
        public string AssetWorksPartLocationCode { get; set; }
        public string AssetWorksUnitPrice { get; set; }

        public DateTime? AssetWorksPartDateInserted { get; set; }
        public DateTime? AssetWorksPartDateRequired { get; set; }

        public int? AssetWorksPartQuantityRequested { get; set; }
        public int? AssetWorksPartQuantityOnHand { get; set; }
        public int? AssetWorksPartQuantityOnOrder { get; set; }
        public int? AssetWorksPartQuantityCommited { get; set; }

        public int BidQuoteId { get; set; }
        public string BidQuoteDomainKey { get; set; }
        public string BidQuoteStatus { get; set; }
        public string BidQuoteStatusDisplayName => ((BidQuoteStatus)Enum.Parse(typeof(BidQuoteStatus), BidQuoteStatus)).ToEnumDisplayName();
        public string BidQuoteStatusDisplayDescription => ((BidQuoteStatus)Enum.Parse(typeof(BidQuoteStatus), BidQuoteStatus)).ToEnumDisplayDescription();

        public int VendorBidsCount => VendorBids.Count();
        public int VendorInPartsCount => VendorInParts.Count();
        public int BidsSelectedCount => VendorBids.Count(x => x.IsSelected == true);

        public int LowestBidder => VendorBids.Any() ? VendorBids.OrderBy(p => p.UnitPrice ?? 0).Select(x => x.VendorId).FirstOrDefault() : 0;
        public int SelectedBidder => VendorBids.Any() ? VendorBids.Where(x => x.IsSelected != null && x.IsSelected == true).Select(x => x.VendorId).FirstOrDefault() : 0;

        public int? RequestedQuantity { get; set; }

        public int? PurchaseOrderQuantity { get; set; }

        public VendorPartBidDto VendorBid { get; set; }
        public IEnumerable<VendorPartBidDto> VendorBids { get; set; }
        public VendorInPartDto VendorInPart { get; set; }
        public IEnumerable<VendorInPartDto> VendorInParts { get; set; }
    }
}