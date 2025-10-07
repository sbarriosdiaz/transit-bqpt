using System;
using System.Collections.Generic;
using System.Linq;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class BidQuoteDto : BaseDto
    {
        public string AssetWorksBidQuoteId { get; set; }

        public DateTime? AssetWorksDateInserted { get; set; }
        public DateTime? AssetWorksDateApproval { get; set; }
        public DateTime? AssetWorksDateRequired { get; set; }
        public DateTime? AssetWorksDateRequested { get; set; }

        public DateTime? BidScheduledStartTime { get; set; }
        public DateTime? BidScheduledEndTime { get; set; }
        public DateTime? BidStartTime { get; set; }
        public DateTime? BidEndTime { get; set; }

        public DateTime? BidImportedTime { get; set; }
        public DateTime? BidClosedTime { get; set; }

        public string Status { get; set; }
        public string StatusDisplayName => Status.ParseEnum<BidQuoteStatus>().ToEnumDisplayName();
        public string StatusDisplayDescription => Status.ParseEnum<BidQuoteStatus>().ToEnumDisplayDescription();
        public string StatusOrder => Status.ParseEnum<BidQuoteStatus>().ToEnumOrder();

        public int? DisplayOrder { get; set; }

        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int PartCount => Parts.Any() ? Parts.Count() : 0;
        public int VendorBidCount => Parts.Any() ? Parts.Select(v => v.VendorBidsCount).Sum() : 0;
        public int VendorInPartCount => Parts.Any() ? Parts.Select(v => v.VendorInPartsCount).Sum() : 0;
        public int VendorSelectedCount => Parts.Any() ? Parts.Select(v => v.BidsSelectedCount).Sum() : 0;

        public bool CanUploadInvoice { get; set; } = false;

        public IEnumerable<BidQuoteNoteDto> Notes { get; set; }
        public IEnumerable<BidQuoteStatusHistoryDto> StatusHistory { get; set; }
        public IEnumerable<BidQuotePartDto> Parts { get; set; }
        public IEnumerable<VendorBidQuoteAttachmentDto> Attachments { get; set; }

        public VendorBidQuoteAttachmentDto Attachment => Attachments.Any() ? Attachments.OrderByDescending(a => a.Id).FirstOrDefault() : null;
    }
}