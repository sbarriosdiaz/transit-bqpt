using System;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class InvoiceDto
    {
        public string AttachmentId { get; set; }
        public string FileExtenstion { get; set; }
        public string FileFriendlyName { get; set; }
        public string ProjectFileType { get; set; }
        public int BidQuoteId { get; set; }
        public string Status { get; set; }
        public string BlobStatus { get; set; }
        public double FileSize { get; set; }
        public bool CanRequestDeliveryOnDemand { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string FileGroupDescription { get; set; }
        public string NormalizedFile => $"{FileFriendlyName}".ToSlug().ToLowerInvariant();

        public string DownloadReadyFile => $"{NormalizedFile}{FileExtenstion}";
    }
}