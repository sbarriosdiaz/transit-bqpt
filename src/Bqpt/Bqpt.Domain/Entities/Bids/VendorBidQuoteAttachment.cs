namespace Bqpt.Domain
{
    public class VendorBidQuoteAttachment : Entity<int>
    {
        public string Comment { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int BidQuoteId { get; set; }
        public BidQuote BidQuote { get; set; }

        public string AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}