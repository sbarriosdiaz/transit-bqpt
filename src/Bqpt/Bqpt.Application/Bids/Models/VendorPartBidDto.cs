using System;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorPartBidDto : BaseDto
    {
        public string BidQuoteDomainKey { get; set; }

        public int? QuantityAvailable { get; set; }
        public int? ItemsPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? EstimateDeliveryDate { get; set; }
        public string Comment { get; set; }
        public decimal? Core { get; set; }
        public string AssetWorksPartNumber { get; set; }
        public int? RequestedQuantity { get; set; }
        public string AssetWorksPartDescription { get; set; }

        public bool? IsSelected { get; set; }

        public int VendorId { get; set; }
        public VendorDto Vendor { get; set; }

        public int AttachmentId { get; set; }
        public VendorBidQuoteAttachmentDto Attachment { get; set; }

        public int BidQuotePartId { get; set; }

        public string BidQuoteStatus { get; set; }
    }
}