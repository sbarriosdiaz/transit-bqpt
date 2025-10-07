using System;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorBidQuoteAttachmentDto : BaseDto
    {
        public string AttachmentId { get; set; }

        public string BlobUrl { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string FileFriendlyName { get; set; }

        public string Status { get; set; }

        public int? VendorId { get; set; }

        public int? BidQuoteId { get; set; }

        public string Comment { get; set; }

        public double FileSize { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}