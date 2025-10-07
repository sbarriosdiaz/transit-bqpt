using System;
using System.Collections.Generic;
using System.Linq;

namespace Bqpt.Application
{
    public class AssetWorksBidQuoteDto
    {
        public string AssetWorksBidQuoteId { get; set; }
        public DateTime? AssetWorksDateInserted { get; set; }
        public DateTime? AssetWorksDateApproval { get; set; }
        public DateTime? AssetWorksDateRequired { get; set; }
        public DateTime? AssetWorksDateRequested { get; set; }
        public int? AssetWorksPartCount { get; set; }
        public int? AssetWorksPartCountWithNoCC { get; set; }
        public int? AssetWorksNoVendorCCMatch { get; set; }
        public int? AssetWorksVendorsNoEmail { get; set; }

        public int? TotalCountNoVendorMatch => Parts.Any() ? Parts.Count(x => x.NoVendorMatch) : 0;
        public int? TotalCountNoCC => Parts?.Count(x => string.IsNullOrEmpty(x.AssetWorksPartCommodityCode));
        public int? TotalCountParts => Parts?.Count();

        public IEnumerable<VendorDto> VendorsWithNoEmail => Parts.Any() ? Parts.SelectMany(d => d.VendorsWithNoEmail) : Enumerable.Empty<VendorDto>();
        public int? VendorsWithNoEmailCount => VendorsWithNoEmail.Any() ? VendorsWithNoEmail.Select(x => x.AssetWorksVendorNumber).Distinct().Count() : 0;
        public IEnumerable<AssetWorksBidQuotePartDto> Parts { get; set; }
    }
}