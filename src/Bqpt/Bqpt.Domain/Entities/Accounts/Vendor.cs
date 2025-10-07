using System.Collections.Generic;

namespace Bqpt.Domain
{
    public class Vendor : Entity<int>
    {
        public string AssetWorksVendorNumber { get; set; }
        public string AssetWorksVendorAccountingSystemNumber { get; set; }
        public string AssetWorksVendorName { get; set; }
        public string AssetWorksVendorContactName { get; set; }
        public string AssetWorksVendorAddress1 { get; set; }
        public string AssetWorksVendorAddress2 { get; set; }
        public string AssetWorksVendorAddress3 { get; set; }
        public string AssetWorksVendorAddress4 { get; set; }
        public string AssetWorksVendorEmail { get; set; }
        public string AssetWorksVendorPhone { get; set; }
        public string AssetWorksVendorFax { get; set; }
        public string CommodityCodes { get; set; }

        public ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
        public ICollection<VendorPartBid> Bids { get; set; } = new HashSet<VendorPartBid>();
        public ICollection<VendorInPart> VendorInParts { get; set; } = new HashSet<VendorInPart>();
        public ICollection<VendorBidQuoteAttachment> VendorBidQuoteAttachments { get; set; } = new HashSet<VendorBidQuoteAttachment>();
    }
}