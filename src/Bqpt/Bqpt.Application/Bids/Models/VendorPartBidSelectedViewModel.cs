using Bqpt.Common;

namespace Bqpt.Application
{
    public class VendorPartBidSelectedViewModel : BaseViewModel<int>
    {
        public string BidQuoteDomainKey { get; set; }
        public int? VendorId { get; set; }
        public int? BidQuotePartId { get; set; }

        public bool? IsSelected { get; set; }
    }
}