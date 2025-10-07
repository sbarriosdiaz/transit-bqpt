using System;
using System.Collections.Generic;
using System.Linq;

namespace Bqpt.Application
{
    public class AssetWorksBidQuotePartDto
    {
        public string AssetWorksBidQuoteId { get; set; }
        public string AssetWorksPartNumber { get; set; }
        public string AssetWorksPartSuffix { get; set; }

        public string AssetWorksPartCommodityCode { get; set; }
        public string AssetWorksPartManufacturerPartNumber { get; set; }

        public string AssetWorksPartDescription { get; set; }
        public string AssetWorksUnitPrice { get; set; }
        public string AssetWorksPartCategory { get; set; }
        public string AssetWorksPartLineNumber { get; set; }
        public string AssetWorksMinPartLineNumber { get; set; }
        public string AssetWorksPartLocationCode { get; set; }

        public DateTime? AssetWorksPartDateInserted { get; set; }
        public DateTime? AssetWorksPartDateRequired { get; set; }

        public int? AssetWorksPartQuantityRequested { get; set; }
        public int? AssetWorksPartQuantityOnHand { get; set; }
        public int? AssetWorksPartQuantityOnOrder { get; set; }
        public int? AssetWorksPartQuantityCommited { get; set; }

        public int VendorCount => Vendors.Count();
        public bool NoVendorMatch => !Vendors.Any() && !string.IsNullOrEmpty(AssetWorksPartCommodityCode);

        public int? VendorNoEmailCount => Vendors.Any() ? Vendors.Count(v => string.IsNullOrEmpty(v.AssetWorksVendorEmail)) : 0;
        public IEnumerable<VendorDto> VendorsWithNoEmail => Vendors.Any() ? Vendors.Where(v => string.IsNullOrEmpty(v.AssetWorksVendorEmail)) : Enumerable.Empty<VendorDto>();

        public IEnumerable<VendorDto> Vendors { get; set; }
    }
}