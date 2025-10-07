using System.Collections.Generic;
using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorDto : BaseDto
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
        public int VendorId { get; set; }
        public int ContactCount => Contacts.Count;

        public ICollection<ContactDto> Contacts { get; set; }
    }
}